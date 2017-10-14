using System;
using System.Threading;
using BMCore.Contracts;
using BMCore.DbService;

namespace BMCore.Engine
{
    public class EngineHelper
    {
        public static void ExecuteAllExchanges(IExchange[] exchanges, BMDbService dbService, decimal threshold)
        {
            for (var i = 0; i < exchanges.Length; i++)
            {
                var baseExchange = exchanges[i];
                for (var j = 0; j < exchanges.Length; j++)
                {
                    var arbExchange = exchanges[j];

                    if (baseExchange != arbExchange)
                    {
                        try
                        {
                            Console.WriteLine("Starting: {0} {1}", baseExchange.Name, arbExchange.Name);
                            var engine = new TradingEngine(baseExchange, arbExchange, dbService, threshold);
                            engine.AnalyzeMarkets().Wait();
                            Console.WriteLine("Completed: {0} {1}", baseExchange.Name, arbExchange.Name);

                        }
                        catch (Exception e)
                        {
                            //Console.WriteLine(e);
                            Console.WriteLine("Error: {0} {1}", baseExchange.Name, arbExchange.Name);
                            dbService.LogError(baseExchange.Name, arbExchange.Name, "", "Main", e.Message, e.StackTrace);

                        }
                        finally
                        {
                            //Thread.Sleep(1000 * 60);
                            Thread.Sleep(100);
                        }
                    }

                }
            }
        }

        public static void ExecuteExchangePair(IExchange baseExchange, IExchange arbExchange, BMDbService dbService, decimal threshold)
        {
            try
            {
                Console.WriteLine("Starting: {0} {1}", baseExchange.Name, arbExchange.Name);
                var engine = new TradingEngine(baseExchange, arbExchange, dbService, threshold);
                engine.AnalyzeMarkets().Wait();
                Console.WriteLine("Completed: {0} {1}", baseExchange.Name, arbExchange.Name);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                Console.WriteLine("Error: {0} {1}", baseExchange.Name, arbExchange.Name);
                dbService.LogError(baseExchange.Name, arbExchange.Name, "", "Main", e.Message, e.StackTrace);

            }
        }

    }
}