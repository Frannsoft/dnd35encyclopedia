using System;
using System.Threading.Tasks;
using System.Windows;
using Windows.ApplicationModel.Store;

namespace DnDNavigator.Runtime.DataAccess
{
    /// <summary>
    /// Handles all in-app purchasing license logic.
    /// 
    /// This class is static as it's job is critical and only needs to ever
    /// exist once in memory to perform its job.
    /// </summary>
    public static class LicenseService
    {
        private const string LICENSE_KEY = "Premium"; //TODO: change this to actual name

        /// <summary>
        /// Basically a cache of whether or not we should fully reach out to the licensing service from ms
        /// a check the license on the device.  If true, then yes ping the service.  If false, just pull the 
        /// current value 'IsLicensed'
        /// </summary>
        public static bool RefreshLicenseCheckNeeded { get; set; }

        /// <summary>
        /// MISSION CRITICAL.  IF SET, BYPASSES LICENSE CHECK AND RETURNS APP IS LICENSED FOR PREMIUM.  FOR 
        /// DONATE VERSION USE ONLY!  SET IN APP.XAML.CS BOOTER METHODS.
        /// </summary>
        public static bool IsDonate { get; set; }

        private static bool isLicensed;
        /// <summary>
        /// True if the user's device is licensed.  False if it is not.  Getter performs a check each time
        /// to see if the ms license service should be pinged or if using the currently stored local value is ok.
        /// </summary>
        public static bool IsLicensed
        {
            get
            {
                //bypass full check and return true if Donate version of app
                if (IsDonate)
                {
                    return true;
                }

                if (RefreshLicenseCheckNeeded)
                {


                    bool? storedLicense = IsolatedStorage.LicenseStatus;
                    if (storedLicense != null)
                    {
                        isLicensed = storedLicense.Value;
                    }
                    else
                    {
                        isLicensed = CheckLicense();
                    }
                    RefreshLicenseCheckNeeded = false;
                    return isLicensed;
                }
                else
                {
                    return isLicensed;
                }
            }
            private set
            {
                isLicensed = CheckLicense();
                IsolatedStorage.LicenseStatus = isLicensed;
            }
        }

        private static bool CheckLicense()
        {
            bool result = false;

            //TODO - actually check license and ping the ms service to do so
            ProductLicense license = CurrentApp.LicenseInformation.ProductLicenses[LICENSE_KEY];
            if (license != null)
            {
                result = license.IsActive;
            }
            else
            {
                MessageBox.Show("Unable to obtain license data");
            }
            return result;
        }

        public static void FulfillLicense()
        {
            var productLicenses = CurrentApp.LicenseInformation.ProductLicenses;

            MaybeGiveMePremium(productLicenses[LICENSE_KEY]);
            RefreshLicenseCheckNeeded = true;

        }

        private static void MaybeGiveMePremium(ProductLicense productLicense)
        {
            if (!productLicense.IsConsumable && productLicense.IsActive)
            {
                CurrentApp.ReportProductFulfillment(productLicense.ProductId);
            }
        }

        public static async Task BeginPurchaseProduct()
        {
            try
            {
                //kick off purchase; don't ask for a receipt when it returns
                await CurrentApp.RequestProductPurchaseAsync(LICENSE_KEY, false);

                //now that purchase is done, give the user the goods they paid for
                FulfillLicense();
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
