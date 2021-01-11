using Common.Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Net;
using log4net;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ILog _Log = LogManager.GetLogger("LOGGER");

            var login = MyOptions.login;
            var password = MyOptions.password;
            var connectionString = @"AuthType=OAuth;Url=https://homertrialtest.api.crm4.dynamics.com/;" + login + password +"Password=Sdf034a$q;AppId=51f81489-12ee-4a9e-aaae-a2591f45987d;RedirectUri=app://58145B91-0C36-4500-8554-080854F2AC97;";
            CrmServiceClient client = new CrmServiceClient(connectionString);

            if(client.LastCrmException != null)
            {
                Console.WriteLine(client.LastCrmException);
                _Log.Info("Ошибка соединения");
            }

            var service = (IOrganizationService)client;

            RetriveContact(service, _Log);

            Console.ReadKey();
        }

        public static void RetriveContact(IOrganizationService _service, ILog _Log)
        {
            // Выбираем тех у кого есть телефон1 и email1 и нет связи со средствами связи

            var query = new QueryExpression("contact");
            query.Distinct = true;

            query.ColumnSet.AddColumns("fullname", "telephone1", "contactid");
            query.AddOrder("fullname", OrderType.Ascending);

            var query_Criteria_0 = new FilterExpression();
            query.Criteria.AddFilter(query_Criteria_0);

            var query_Criteria_0_0 = new FilterExpression();
            query_Criteria_0.AddFilter(query_Criteria_0_0);

            query_Criteria_0_0.FilterOperator = LogicalOperator.Or;
            query_Criteria_0_0.AddCondition("telephone1", ConditionOperator.NotNull);
            query_Criteria_0_0.AddCondition("emailaddress1", ConditionOperator.NotNull);
            var query_Criteria_1 = new FilterExpression();
            query.Criteria.AddFilter(query_Criteria_1);

            query_Criteria_1.AddCondition("am", "ptest_contactid", ConditionOperator.Null);

            var am = query.AddLink("ptest_communication", "contactid", "ptest_contactid", JoinOperator.LeftOuter);
            am.EntityAlias = "am";

            EntityCollection result = _service.RetrieveMultiple(query);

            var listContacts = result.Entities;
            _Log.Info($"Выполнен fetchXml запрос. Количество записей {listContacts.Count}");
            foreach (var contact in listContacts)
            {
                string email, phone;
                contact.TryGetAttributeValue("emailaddress1", out email);
                contact.TryGetAttributeValue("telephone1", out phone);
                _Log.Info($"Начата запись новых элементов \"Средства связи\"");
                //объевление нового элемента для сохранения
                Entity communication = new Entity("ptest_communication");
                //Создание связи с контактом
                EntityReference conctactRef = new EntityReference("contact", contact.GetAttributeValue<Guid>("contactid"));

                //Заполнение полей
                communication["ptest_contactid"] = conctactRef;
                communication["ptest_name"] = contact["fullname"];

                if (phone.Length > 0)
                {
                    communication["ptest_phone"] = phone;
                    communication["ptest_type"] = new OptionSetValue(234260000);
                    communication["ptest_main"] = true;
                    Guid communicationId = _service.Create(communication);
                    _Log.Info($"Создано новое средство связи с типом телефон. Id: {communicationId}");
                }
                if (email.Length > 0)
                {
                    communication["ptest_email"] = email;
                    communication["ptest_type"] = new OptionSetValue(234260001);
                    communication["ptest_main"] = false;
                    Guid communicationId = _service.Create(communication);
                    _Log.Info($"Создано новое средство связи с типом email. Id: {communicationId}");
                }
            }
        }
    }
}
