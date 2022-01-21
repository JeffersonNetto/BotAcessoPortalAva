using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Serilog;
using System;
using System.Collections.Generic;

namespace BotAcessoPortalAVA
{
    class Program
    {
        private static string login, senha;
        private static IEnumerable<IConfigurationSection> links;
        static void Main()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

            var config = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true).Build();            

            login = config["Credenciais:Login"];
            senha = config["Credenciais:Senha"];
            links = config.GetSection("Links").GetChildren();

            try
            {
                Log.Information("Iniciando...");

                Executar();

                Log.Information("Processo finalizado com sucesso");

                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Ocorreu um erro");
            }
            finally
            {
                Log.CloseAndFlush();
            }            
        }

        private static void Executar()
        {            
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");            

            using IWebDriver driver = new ChromeDriver(options);

            driver.Navigate().GoToUrl("https://avaeduc.com.br");

            IWebElement elementUsername = driver.FindElement(By.Name("username"));

            elementUsername.Click();

            elementUsername.SendKeys(login);

            IWebElement elementPassword = driver.FindElement(By.Name("password"));

            elementPassword.Click();

            elementPassword.SendKeys(senha);

            elementPassword.Submit();

            foreach (var item in links)
                driver.SwitchTo().NewWindow(WindowType.Tab).Navigate().GoToUrl(item.Value);
        }
    }
}
