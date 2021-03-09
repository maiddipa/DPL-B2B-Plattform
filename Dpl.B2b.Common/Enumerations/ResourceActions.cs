using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dpl.B2b.Common.Enumerations
{
    public enum ResourceAction
    {
        // all
        Create = 0,
        Update = 1,
        Read = 2,
        Delete = 3,

        // organization
        CreateCustomer = 100,

        // organization / customer
        UpdateCustomer = 101,
        DeleteCustomer = 102,

        CreateDivision = 200,

        // organization / customer / division
        UpdateDivision = 201,
        DeleteDivision = 202,

        CreateVoucher = 304,
        CancelVoucher = 305,
        SubmitVoucher = 306,
        ReadVoucher = 307,

        CreateLoadCarrierReceipt = 404,
        CancelLoadCarrierReceipt = 405,
        ReadLoadCarrierReceipt = 407,

        CreateOrder = 504,
        UpdateOrder = 501,
        CancelOrder = 505,
        ReadOrder = 507,

        CancelOrderLoad = 705,
        ReadOrderLoad = 707,

        CreateExpressCode = 800,
        ReadExpressCode = 802,
        CancelExpressCode = 805,

        // organization / customer / posting account

        CreateBalanceTransfer = 604,
        ReadBalanceTransfer = 601,
        CancelBalanceTransfer = 605,

        ReadAccountingRecords = 901,

        CreateLivePoolingSearch = 1001,
        ReadLivePoolingOrders = 1002,

        CreateSorting = 1101
    }

    namespace ResourceActions
    {
        public static class Division
        {
            public static ResourceAction[] Admin;
            public static ResourceAction[] Standard;

            static Division()
            {
                var standardActions = new ResourceAction[]
                {
                    ResourceAction.CreateVoucher,
                    ResourceAction.CancelVoucher,
                    ResourceAction.SubmitVoucher,
                    ResourceAction.ReadVoucher,

                    ResourceAction.CreateLoadCarrierReceipt ,
                    ResourceAction.CancelLoadCarrierReceipt,
                    ResourceAction.ReadLoadCarrierReceipt,

                    ResourceAction.CreateOrder,
                    ResourceAction.UpdateOrder,
                    ResourceAction.CancelOrder,
                    ResourceAction.ReadOrder,

                    ResourceAction.CreateBalanceTransfer,
                    ResourceAction.ReadBalanceTransfer,
                    ResourceAction.CancelBalanceTransfer,

                    ResourceAction.CancelOrderLoad,
                    ResourceAction.ReadOrderLoad,

                    ResourceAction.CancelOrderLoad,
                    ResourceAction.ReadOrderLoad,

                    ResourceAction.ReadAccountingRecords,

                    ResourceAction.CreateLivePoolingSearch,
                    ResourceAction.ReadLivePoolingOrders,

                    ResourceAction.CreateSorting,
                };

                Standard = standardActions;

                var adminActions = new ResourceAction[]
                {
                    ResourceAction.Create,
                    ResourceAction.Update,
                    ResourceAction.Delete,
                    ResourceAction.Read,
                }.Concat(standardActions).Distinct().ToArray();

                Admin = adminActions;
            }
        }

        public static class Customer
        {
            public static ResourceAction[] Admin;
            public static ResourceAction[] Standard;

            static Customer()
            {
                var standardActions = new ResourceAction[]
                {
                }.Concat(Division.Standard).Distinct().ToArray();

                Standard = standardActions;

                var adminActions = new ResourceAction[]
                {
                    ResourceAction.Create,
                    ResourceAction.Update,
                    ResourceAction.Delete,
                    ResourceAction.Read,

                    ResourceAction.CreateDivision,
                    ResourceAction.UpdateDivision,
                    ResourceAction.DeleteDivision,
                }.Concat(standardActions).Concat(Division.Admin).Distinct().ToArray();

                Admin = adminActions;
            }
        }
        public static class Organization
        {
            public static ResourceAction[] Admin;
            public static ResourceAction[] Standard;

            static Organization()
            {
                var standardActions = new ResourceAction[]
                {

                }.Concat(Customer.Standard).Distinct().ToArray();

                Standard = standardActions;

                var adminActions = new ResourceAction[]
                {
                    ResourceAction.Create,
                    ResourceAction.Update,
                    ResourceAction.Delete,
                    ResourceAction.Read,

                    ResourceAction.CreateCustomer,
                    ResourceAction.UpdateCustomer,
                    ResourceAction.DeleteCustomer,
                }.Concat(standardActions).Concat(Customer.Admin).Distinct().ToArray();

                Admin = adminActions;
            }
        }
    }
}
