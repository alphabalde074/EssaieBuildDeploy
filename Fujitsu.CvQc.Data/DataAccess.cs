using Azure.Data.AppConfiguration;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Fujitsu.CvQc.Data.Entities;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Fujitsu.CvQc.Data;
public class DataAccess : IDataAccess
{
    private static string _urlVoute;
    private static string _urlAppConfig;
    private static string _nomEnvir = "Envir";
    private static string _Envir = "Dev";
    private static string _nomCleUrlVoute = "UrlVoute";
    private static string _nomClefUrlAppConfig = "UrlAppConfig";
    private static string _nomClef = "DevBD";
    private static ConfigurationClient _clientAppConfig;
    public void ConfigDataContext(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        string connectionString = string.Empty;
        if (ExisteDansAzure())
        {
            ObtenirNomEnvir();
            ObtenirNomSecret();
            ObtenirUrlAppConfig();
            CreateurClientAppConfig();
            ObtenirUrlVoute();

            connectionString = _clientAppConfig.GetConfigurationSetting(_nomClef).Value.Value;

            var client = new SecretClient(new Uri(_urlVoute), new DefaultAzureCredential());
            KeyVaultSecret secret = client.GetSecret(_nomClef);
            SecretBD secretBD = JsonConvert.DeserializeObject<SecretBD>(secret.Value);

            connectionString = connectionString.Replace("=U", "=" + secretBD.U).Replace("=P", "=" + secretBD.P);

        }
        else
        {


            connectionString = configuration.GetConnectionString("DataContext");
        }

    

        if (String.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidDataException("connectionString is null or empty");
        }
        else
        {
            connectionString = FormatConnectionString(configuration, connectionString);
        }

        serviceCollection.AddDbContext<DataContext>(options =>
            options.UseSqlServer(
                connectionString,
                providerOptions =>
                {
                    providerOptions.EnableRetryOnFailure();
                    providerOptions.MigrationsAssembly("Fujitsu.CvQc.Data");
                    providerOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                }
            ).LogTo(Console.WriteLine, LogLevel.Information)
        );
    }

    private void ObtenirUrlVoute()
    {
        //Aller chercher l'info dans la clef dans App Config du App Service
        //Le URL sera spécifié dans le déploiement dans Azure DevOps
        _urlVoute = Environment.GetEnvironmentVariable(_nomCleUrlVoute);
    }

    private void ObtenirNomEnvir()
    {
        //Aller chercher l'info dans la clef dans App Config du App Service
        //Le URL sera spécifié dans le déploiement dans Azure DevOps
        _nomEnvir = Environment.GetEnvironmentVariable(_Envir);
    }

    private void ObtenirUrlAppConfig()
    {
        //Aller chercher l'info dans la clef dans App Config du App Service
        //Le URL sera spécifié dans le déploiement dans Azure DevOps
        _urlAppConfig = Environment.GetEnvironmentVariable(_nomClefUrlAppConfig);
    }

    private void CreateurClientAppConfig()
    {
        _clientAppConfig = new ConfigurationClient(new Uri(_urlAppConfig), new DefaultAzureCredential());
    }


    private static void ObtenirNomSecret()
    {
        //Aller chercher l'info dans la clef dans App Config du App Service
        //Le URL sera spécifié dans le déploiement dans Azure DevOps
        _nomClef = string.Format(_nomClef, _nomEnvir);
    }
    private bool ExisteDansAzure()
    {
        bool existe = false;
        string nomClef = "WEBSITE_HOSTNAME";
        //Aller chercher l'info dans la clef dans App Config du App Service
        //Le URL sera spécifié dans le déploiement dans Azure DevOps
        if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(nomClef)))
        {
            existe = true;
        }
        return existe;


    }

    public static string FormatConnectionString(IConfiguration configuration, string connectionString)
    {
        var externalSettingPath = configuration["ExternalSettingPath"];
        if (!String.IsNullOrWhiteSpace(externalSettingPath))
        {
            // Extract missing parameters from external file and replace in connection string 
            //   dataContext.val1: username, 
            //   dataContext.val2: partial password (passowrd ending kept inside the connectionString)
            using (StreamReader reader = new StreamReader(externalSettingPath))
            {
                string json = reader.ReadToEnd();
                dynamic? settings = JsonConvert.DeserializeObject(json);
                if (settings != null)
                {
                    string val1 = settings.dataContext.val1.Value;
                    string val2 = settings.dataContext.val2.Value;

                    if (!string.IsNullOrEmpty(val1))
                    {
                        connectionString = connectionString.Replace("{val1}", val1);
                    }
                    if (!string.IsNullOrEmpty(val2))
                    {
                        connectionString = connectionString.Replace("{val2}", val2);
                    }
                }
                reader.Close();
            }
        }
        else
        {
            // Extract missing parameters from system variables and replace in connection string 

            // TODO: Si externalSettingPath est null, lire val1 et val2 dans les variables d'environment du système à la place (ou autre manière appropriée).
            //         pourrait être nécessaire en production où autre environment sans fichier pour contenir l'utilisateur et le mot de passe de la connection à bd
        }

        return connectionString;
    }
}
