﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was not generated by a tool. but for stylecop suppression.
// </auto-generated>
//------------------------------------------------------------------------------
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using StrixIT.Platform.Core;
using Moq;
using StrixIT.Platform.Modules.Cms;
using System.Web;

namespace StrixIT.Platform.Modules.Cms.Tests
{
    [TestClass()]
    public class ImageConverterTests
    {
        [TestInitialize]
        public void Init()
        {
            TestHelpers.MockUtilities();
        }

        [TestMethod()]
        public void ImageAsBase64ShouldReturnProperBase64String()
        {
            ImageConverter target = new ImageConverter();
            string path = StrixPlatform.Environment.WorkingDirectory + @"\TestFiles\Strix_losuiltje.png";
            int width = 100;
            int height = 100;
            string expected = "data:image/.png;base64,iVBORw0KGgoAAAANSUhEUgAAAFcAAABkCAYAAADzJqxvAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAABBVSURBVHhe7Z0JVBRHGsdxN3vmkOkBjHc8YozXZjcb6RlQNGpMoptoIolGnJ4BRdFgRFHBi6BG8cIVVBS88lS8QU1EUSGK9423RlFRoyK3b3eTzW62tr6erp4erIFp6OlpSOq93wOGruP7T3Xdh0dtcybjC60sRs8gjmXmcQZmJ+aC2aB7iPkX/v0n/Pl/8O/l+OcdzBGzgVlrYj2jzb5Mr8GdmReEYH5x4AIDPX4dbNR3x8ItxULdxqDqgsP4EQuewxl044b46ZoJUfz83KDXn/Mys8wkyIE0oaRYjAwa3l2Pwt/yQiN7eqHh3fT8Z7RnRVjmvzjsPZzBs09MjMevhGjrtgthX2A41jOOY3VPaKJM6OeDlo1rinYvaYXObn0F3T3YHpWe64ieXOhkR+Gpjigvqx06ntoGbZvTAi0Y0RiFven1VHgAztG5nEHfD0dfz5qKOuYg9+DXNRTnpsKKxo/q5YW2zW2B8va3e0pEOZSc7YhObGiD5g9vZBe+DV22yU/XXkhS3XCcb/2XwDC6wVYi+njzAhfhHEkTzhku7miLFoQ1RkO76Klx8LDM9/hLjgoI8HhGSF7tdUM6e76PX8tiqqEUIv/mwxcHNPEcUXymI1o1qRmy+NHDpIEF3hvi69NASGatc/Vwbp0IFQsYY/HzQsnTR6IjmVvRxZMH0MVTB9HRfWloc9IMFGN5E5mNttwW4s+gjMWtqEJW5MHRDig2qKGdcGE9m6Ol04ahzM3L0dnDmejymUP8z8wtySh6ECs+h7/0vGCDroOQ3trhYjygfGUWYAP+B0aM7NUCi7oFPXlS5pDzx7PRNC5ANBxaA5lLKxe44EQHNG3gi6KfoV1fRKkJU1HBw3vUOICixw9QUsww0Q/+8gs5P4YVkq5th6w5diFJ/Lj+f0LXL56kGlqRkpJCtDw2zCYWLjtz09pShS0/3wktHNlYfHZU71bo3NF91HArUl5eitJXzkXBft5W/yxTEsIynQUTtOs4IzMZJ5jPsVEfd0Z3865SDXQEGJ6EX2ki2sT+Pqjo9NOVXFZya/GZ4W82xUXNQWp4lbF3SwoK9vchAhcMfcOrjWCG9pzFwAwgZez4D/+M7t66RjWqKooLH6FJnxhE8aAVIRX28cmOaMy7Qq4z6FHmpuXUcJxhD/YbjOsDCAtXcpe5gPqegjnacUP8vdpgYUshkfCK3rh8hmqMs8ArbjFajQ7v7Y0F7SCKu3NhS0FYBn0x/F1UVlZCDcNZNidNF8PDbII6QzDL/S70dY/fCIMo+DXzRsezdlCNkMu8MQNEozOTrJVbWW4nvhcHn4H4Z4/spfqVA3w5C8d/IsbF+TJmwTT3OxPLRJCErV0YTTWgOpw6uEs0eIapIS/uuW2viJ/FmLvzZTTNr1we3r+NxvbrSMIuGtzFq6FgnvvcQKO+EekkQBuyqPAhNfHVoay0SDQ4xF+P7h9qj9ZMbSaK+/XaBKq/6nJsfzrfHoewOVa3UjDRfQ43uxIgMZAoyGm0RNeEVXERopjZKa1R9IcN+N9DcC1/5+Zlqp+aQIoHnGF+dGsHY2hnpgkW9x+QmPljAhV7RaUc3btNFBfGDaD3Br/DW+KK+G5eOct3RniBWSZVMFV9Z2Z1MyERkGuhS0tLbE35Lv8mX0lCPERYYPn0MOrzSiDpwf072ODZXDBXPTeAbfIH3C68B4mYOfwdp3MRPAc1fM6uDagQd0Vpz0gpLy/he3lEVMLuDUupz0sBv/duX0eluOdH+78jruYeE3tvJoMuVjBZPWfy1b1DDM3e/iU1kTQ2JMaIAk0O8neqApwb3l/0Q4CBGNqzhIIHd9GMYb35Z6FSvH7hBPU5R0wP6cX7xRnoW5iKEsxWx+ECPwUiD+v5Eip4eJeawIoUFz1CI3rYansgJ2Mj9Vkpq2aPsfMDuSq/ispsW/JsOz/zIwKpzzli1/pE0S9nrP+aYLbrXUyAxzNk7it+3EBq4mjAiFRo9yZ2Rh/YuY76rJS0lDg7P6HdG+OwKs/xK74It/MzebAf9TlHwJcHLRLeP8tEC6a73sEABxlDyFi/mJo4R6yc9ZlocOQHr6HHlQwPEvZtXSH6AT7r+yoqr6LLC60Mi2SMODVhCvW5yogeKI797hZMd72zGHSDSKKh8KclzBFlpcW4MktFX69LQA/u5lGfqcih3ZtEkYAJgX+lPmcHrjiz0lajBWM/QluSZqCS4sf05yohecYoa5wsUwBvq2C+ax1n0M+CSEf0aI4KC76jJkxJpN1gYCoXQH1OaTJSl4hxDsZtesF81zoc2SaIMHqQwSUN+YqcObTHTtzYkJ7U55TmdE6GGKfJ4NlVMN+1DjdPjkOEs8L6UBOlNLyRkvJzzHvtqvWay+XOjcu4g2Rt73K+unGC+a51MLEHEc4J70dNlNKsnG2rBAHoEWZuTaY+qyTQxCRdYZOBmSOY71qHC/h8iHDx5GBqopQCxllTE6eKtT4sZwoWusAwtXM8W5lxY0dAkUfa5aqJi9u4d10tLjS1UmaP5g0Dxvb1Rrey2qH9ya1EgYfh9u6x7O1U/0qhurg4slsQ4eJJFmqClGB1fORTwpJpnv3LbQJDh+LUkd3UMJRAfXFZ3VVXiltaWoSiOT9R3KSxTfipdCIuDJqD4OT/G1fMpIajBOoXCwbdZYjQlTk37/oFNLa/OO3CL1cCgUHY6AHWAXMgYbJZ9qiXHOqkuEDe9fMowjavhZbiHBz1gVRYjl9IQvOrFHVWXCDvWq6dwIRFk1wvLFCnxQX4HCwpItTIsYQ6Ly4AAk8I8kWLY4eqJiygSXFhWvzJE2XHHYqLC/ipG9r/XIVbxS0uKuDX3MIUOEyrRLzfAYV2a4RCujRA4e+2QQlRJn5staZLjtyF28SFwe7RfWwrYCpjSpA/yj2WRTVAy7hNXCnBfgwa/74Pmh3ciG8ypUxsir4wN+RXyojP+Pug9JVzVBmmVAq3ijuqpxfavqAF+u6wbRWilPwD7dHy8U35leLEz/q/T8YJrx0Cqy6u2aC7DhGO6K5H32Y6t73p6Lo2aATZJ2bUO7XuQAuoKi6sXTWzugcQYRwuAmhCOiI37RV+9yP4HYYrvRuXTlMN0hKqijsywPs5svsxJaoZVcTKyF7RWiwiZoX11Xz5q6q4JpZpDCsAIcItcS9RBayKRZ+StQt6dOaQ64YLlUBdcX09O1mFYdDeZc7tFasIjM2SXY7zxnxINUorqCuukXmLiAv7bGniOUNCuDX3Qmfjfv4NqmFaQO1iwUTEvZbxKlU4Z4DWAwknQ8MtB1XFxZXZBIgMOg33D7enCucMsDsnNMBaNCyaOIRqmBZQV1zrtlO+zUrbfCeHWRbr1n1Y+6XVsQe1xV0PkY3t68NvW6KJ5ixrY5rzCYep83u3rlONczeqiot7Z99AZDGDXqQKJgfYQAJhAVptkqmcc61dXzj2hCaYHC5sbyuKC8tEaca5G9XEDX+79e9wREUQGYx60QSTw51v2ou9tbQVc6jGuRvVxA0yPOvDscwPENmmWdXrnUkpPNkRBQtDkusU3HmpJCqKq+sAEQF7qjhowhmgtUF6aqvjIqjGuRvVxMWVWW8i7smN1e+dEaTifjkvkmqcu1FNXFwkhBBxv82sfu+MUHACFwvCoT8bEqdRjXM36olr0H0OEUFue3SMPvMgh3uHfqnQRIeLhRUQ0ei3vamn1MkFDmmD8IBd6xKpxrkb9cRldfshoikfN6CKJZdLO23t3Kz0NVTj3I2KOZe5AREp0YEA4BxGIq5Sp4sojSriBgZ4P4eLhXKICJZz0sSSy/7ltu7vpdM5VOPcjSriwnmMnIH5CSLaEd+SKpZc0ue3EMXNz7tCNc7dqCSuvhsRAga6aWLJZc0Ua8Jhx4ySx7coiTriStq4N/bW7ChWAjnRbnSftpqdBVZFXJMvMwciCe2m589QpIklF3IO4+fBPaiGaQFVxMXNsHSIJPI9H7vNH9UFBtrhiGwIM3GSmWqYFlBHXANzCSKJC7ae71VT4Lhs0jvTatcXcLm4Qb0aPItbCmUQycpoZZphpzfblp5mb9dmBwJwubiWzkw7HAF/smhGojLNsF0JtvMYr5w7SjVMC7hcXM6gG0iEOJcm78hrRyRPaMqHB80wZ05pcheuF5f1jOOF6KJHD44o01KY8pF1P1nUQF+qUVpBjZzLn5QPBwUr0VKA82+HdbUOksMBaTSjtEBpSZF44JFLxA026p8nlVnSuJpPSgLn022jYXDUCc0wLQA7klwqrvRwtoOrX6aKJRc4yZmEeeWsdisz6DXCuQ4uE9ciHM4G67rg2gCaWHKZZbFePRD+zssu3RStBERcXDTGC5Io46znNTKPIPC4EHlL9B0B0+lk6f6iiUFUg7TEcKFCwyQLsijjOFb3thBwlXc2OIt0gHzPxmVUg7QEaS2YjcwGQRZlHBZ3FQQMNTucc0ATSy7LIq2vGZzHCKcf0QzSEqK4rG6/IEvNXejrDf9oZpkCCBg27tGEkgvcofPpW9bBGjhvHE6toxmkJWziMpewLMpc+TXE4NmXDxSjVJEgXU2+Y80CqjFaQyJuiWJHDpJ1uFD5KNVKgIlNCBO6vPeqeUGHM8AJpSXFyrRCRHExitxC9Yl/fR2ZjIwPU2amF9YokH3A8ZGDxMSXlRXjXBzPD5jDCnM45RkOt3T2nh0C3IqyZu44flYjpIsP/wXCsa4Hvqr6CNnKkIrLGfQ9BImq7zgjYyYB5qxRpuOwIspakcHes9M5toXO+7atFBMvBdqXld0OVRHpoZZSYKems6ed0rAXVxclSFRtVw+3ErIgMLi+ENqlNLHkAPvOyFgCf3GGZP/DthT7E5oJsJQ//6bzM8KOwqlpq0QqLm4xZAoaVc9Z/Ou3xOUtv0MShgVpYsmF7DkD4KILaeJrk7g40/0w5I3n9YJU8h1uLMdAQDAFAwMsNLHkcHTdy+J0Dr/Xt8KunVqVczEmgy5SkEqeC2zn8Vszaz1KEG4SqelunXs57flFexDe0ICG6Nr5p0+8r23iYoqqdWEzXEBMAkmbZ38HmVzgXt7YwbYrDEf0bI5ig3s+BZyJQ56pyFRTV6ofGg7DwV/S1CHOh1MR+HIqhokrNhhvCRRkc8rVw572gmdo29Zod+SJDrhXZ3/pZh3ksKBb1c5iHbflJyFrUpHdzm5nd+kmBu5P311bwZVYJq7gM/DP7QT8+WbOt343QbrKnfUMBetVMJBrYQsTTbjKgPJ5d2IrFNZDOGoFw7FM7jAt3CvmLgeFM/5mLhBB4ApCmniOgJy6M76leJWWCKtL1+R9jiq5eliEQCwCP/IFLIlogsodtBAgZ4KQsIvnq0Ut+WfhukLSzJJQZDEwFk3d46iWg1v6rWcm6E5IRYEeFJwHBiSMboLmhTZCM00N0aQBDdDo3t5212U5oBCHO782X7Et25lZr7/gsm8ebr+m4tf/PP6dX8BcU/heHMucw19SAjThuIDmvxei/Pk4bHxvzDe4ljuFi4CrUHFhYYrx30/w399jgf6L//5J+Pkj/v8/8RdQiP/Ox1zEHMT+0/BnS3DOjDb76j7mjN6vwfyaEMXP1Hl4/B830v/npa1GMAAAAABJRU5ErkJggg==";
            string actual;
            actual = target.ImageAsBase64(path, width, height);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ResizeShouldResizeImageAndSaveToDisk()
        {
            ImageConverter target = new ImageConverter();
            string path = StrixPlatform.Environment.WorkingDirectory + @"\TestFiles\uiltje.png";
            string resultFile = StrixPlatform.Environment.WorkingDirectory + @"\TestFiles\uiltje_50_50.png";
            System.IO.File.Delete(resultFile);
            int width = 50;
            int height = 50;
            bool overwrite = false;
            target.Resize(path, width, height, overwrite);

            bool result = false;

            if (System.IO.File.Exists(resultFile))
            {
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(resultFile);
                result = bitmap.Width == 50 || bitmap.Height == 50;
            }

            Assert.IsTrue(result);
        }
    }
}