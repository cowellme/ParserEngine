using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ParserEngine
{
    internal class Manager
    {
        /// <summary>
        /// Константа со строкой подключения
        /// </summary>
        private const string connStr = @"server=127.0.0.1;uid=root;pwd=1590;database=lowprice";
        
        /// <summary>
        /// Распаковка команды из формата JSON
        /// </summary>
        /// <param name="file"></param>
        public static void ReadCommand(string file)
        {
            string jsString = File.ReadAllText(file);
            var selCmd = Newtonsoft.Json.JsonConvert.DeserializeObject<SeleniumCommands>(jsString); 
            CommandManger(selCmd);
        }

        /// <summary>
        /// Рспределение команды
        /// </summary>
        /// <param name="selCmd"></param>
        public static void CommandManger(SeleniumCommands selCmd)
        {
            var price = "";
            var uri = "";

            foreach (var selCmdBodyCommand in selCmd.BodyCommands)
            {
                switch (selCmdBodyCommand.Action)
                {
                    case ("Получить"):
                        price = Engine.ParseInfo(selCmdBodyCommand.Value);
                        break;
                    case ("Перейти"):
                        Engine.GoToUri(selCmdBodyCommand.Value); uri = selCmdBodyCommand.Value;
                        break;
                }
            }


            

            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = $@"UPDATE lowprice.prices SET price = '{price}', uri = '{uri}'  WHERE hash = '{selCmd.Hash}' AND platform = '{selCmd.Platform}';";
                    cmd.ExecuteScalar();
                    
                    conn.Close();
                }

            }
            catch
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }
    }
}
