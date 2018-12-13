using DeveloperTest.Config;
using DeveloperTest.Data;
using DeveloperTest.Services;
using Container = StructureMap.Container;
using IContainer = StructureMap.IContainer;

namespace DeveloperTest
{
    public class Bootstrapper
    {
        public static IContainer Container
        {
            get
            {
                var container = new Container(_ =>
                {
                    //Scan should also work for Interface with multiple implementation
                    //but for readability registering manually
                    _.For<IDataStore>().Use<AccountDataStore>();
                    _.For<IDataStore>().Use<BackupAccountDataStore>();
                    _.For<IPaymentType>().Use<BacsPayment>();
                    _.For<IPaymentType>().Use<ChapsPayment>();
                    _.For<IPaymentType>().Use<FasterPayments>();

                    //Single impl
                    _.For<IDataStoreFactory>().Use<DataStoreFactory>();
                    _.For<IAccountFlag>().Use<AccountFlag>();
                    _.For<IPaymentTypeStrategy>().Use<PaymentTypeStrategy>();
                    _.For<IConfigProvider>().Use<ConfigProvider>();
                    _.For<IPaymentService>().Use<PaymentService>();
                });
                
                return container;
            }

        }
    }
}
