using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics.CodeAnalysis;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Interactions;
using OfficeOpenXml;
using System.IO;
using NPOI.SS.Formula.Functions;
using OpenQA.Selenium.DevTools.V131.LayerTree;

namespace BaoDamSelenium
{
    public class Tests
    {
        [SuppressMessage("NUnit.Analyzers.DisposeDriver", "NUnit1032")]
        private IWebDriver driver;
        private string URL = "https://localhost:44375/";
        
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(URL);
            System.Threading.Thread.Sleep(5000);
            //epplus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; 
        }

        [Test]
        public void IntergratedAddToCart()
        {
            ////1. Module Product

            //vào trang chủ click vào sản phẩm để ra trang sản phẩm
            driver.Navigate().GoToUrl("https://localhost:44375/");
            IWebElement elm = driver.FindElement(By.XPath("/html/body/div/header/div[2]/div/div/div/nav/ul[1]/li[2]/a")); //xpath của sản phẩm trên top
            elm.Click();

            // click vao ao nau xem detail
            IWebElement elm1 = driver.FindElement(By.XPath("/html/body/div/div[3]/div/div/div[3]/div/div/div/div[2]/div[1]/div[1]/div[4]/h6/a"));
            elm1.Click();

            // add to cart
            IWebElement elm2 = driver.FindElement(By.XPath("/html/body/div/div[3]/div[2]/div[2]/div/div[4]/div[2]/a"));
            elm2.Click();

            //xử lý thông báo của web khi add thành công
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
            IAlert alert = driver.SwitchTo().Alert();
            Console.WriteLine("Alert text: Thêm sản phẩm vào giở hàng thành công!" + alert.Text);
            alert.Accept();
            //click vào cart để xem sản phẩm và thanh toán

            ////2. Module Cart

            driver.Navigate().Back();

            //click vào cart để xem sản phẩm và thanh toán
            IWebElement elm3 = driver.FindElement(By.XPath("/html/body/div/header/div[2]/div/div/div/nav/ul[2]/li[3]/a"));
            elm3.Click();
            //tăng số lượng
            IWebElement elm4 = driver.FindElement(By.XPath("//*[@id=\"Quantity_1\"]"));
            elm4.Click();
            elm4.Click();
            //cập nhật số lượng
            IWebElement elm5 = driver.FindElement(By.XPath("//*[@id=\"trow_1\"]/td[7]/a[2]"));
            elm5.Click();

            //click thanh toán
            IWebElement elm6 = driver.FindElement(By.XPath("/ html / body / div / div[3] / div / div / div[2] / div[2] / div / a[2]"));
            elm6.Click();
            //điền họ tên khách hàng
            IWebElement elm7 = driver.FindElement(By.XPath("//*[@id=\"myForm\"]/div[1]/input"));
            elm7.SendKeys("LeDoTruongTrong");
            //sdt
            IWebElement elm8 = driver.FindElement(By.XPath("//*[@id=\"myForm\"]/div[2]/input"));
            elm8.SendKeys("0359550283");
            //diachi
            IWebElement elm9 = driver.FindElement(By.XPath("//*[@id=\"myForm\"]/div[3]/input"));
            elm9.SendKeys("3 3/2 q10");
            //email
            IWebElement elm10 = driver.FindElement(By.XPath("//*[@id=\"myForm\"]/div[4]/input")); 
            elm10.SendKeys("truongtrong@gmail.com");
            //chọn COD
            IWebElement elm11 = driver.FindElement(By.XPath("//*[@id=\"myForm\"]/div[5]/select/option[1]"));
            elm11.Click ();
            //Click thanh toán
            IWebElement elm12 = driver.FindElement(By.XPath("//*[@id=\"myForm\"]/div[6]/button\r\n"));
            elm12.Click();
        }
        [Test]
        public void Dangki()
        {
            ////Module 3 : Dang ki dang nhap
            //tìm phần tử account
            IWebElement elm1 = driver.FindElement(By.XPath("/html/body/div/header/div[1]/div/div/div[2]/div/ul/li[3]/a"));

            //tạo action để my account drop
            Actions actions = new Actions(driver);
            actions.MoveToElement(elm1).Perform();

            System.Threading.Thread.Sleep(1000);
            //tìm và click regiter
            IWebElement elm2 = driver.FindElement(By.XPath("/html/body/div/header/div[1]/div/div/div[2]/div/ul/li[3]/ul/li[2]/a"));
            elm2.Click();


            //điền email
            //mỗi lần test thì đổi tên tk một lần tránh trùng token test trước
            IWebElement elm3 = driver.FindElement(By.XPath("//*[@id=\"Email\"]"));
            elm3.SendKeys("truongtrong1234@gmail.com");
            //password
            IWebElement elm4 = driver.FindElement(By.XPath("//*[@id=\"Password\"]"));
            elm4.SendKeys("123456");
            //confirm
            IWebElement elm5 = driver.FindElement(By.XPath("//*[@id=\"ConfirmPassword\"]"));
            elm5.SendKeys("123456");

            //click reigster
            IWebElement elm0 = driver.FindElement(By.XPath("/ html / body / div / div[3] / div / div / form / div[5] / div / input"));
            elm0.Click();

            //tìm lại account vừa r
            IWebElement elm = driver.FindElement(By.XPath("/html/body/div/header/div[1]/div/div/div[2]/div/ul/li[3]/a"));
            
            Actions actions1 = new Actions(driver); 
            actions1.MoveToElement(elm).Perform();
            System.Threading.Thread.Sleep(1000);
            //logout
            IWebElement elm6 = driver.FindElement(By.CssSelector("body > div > header > div.top_nav > div > div > div.col-md-6.text-right > div > ul > li.account > ul > li:nth-child(2) > a"));
            elm6.Click();
        }
        [Test]
        public void LoginUser()
        {
            string excelFilePath = "C:\\Users\\ADMIN\\Desktop\\test.xlsx";

            //mở file excel
            using (var excelPackage = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                //lấy worksheet đầu tiên
                var worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();

                //check ws co ton tai hay khong
                if (worksheet != null)
                {
                    //lay du lieu excel tung hang 1
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++) // start hang 2
                    {
                        //A2
                        string username = worksheet.Cells[row, 1].Value?.ToString();
                        //B2
                        string Password = worksheet.Cells[row, 2].Value?.ToString();
                        //dieu kien
                        bool loginSuccess = false;

                        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(Password))
                        {
                            driver.Navigate().GoToUrl("https://localhost:44375/account/login");
                            driver.FindElement(By.Id("UserName")).Click();
                            driver.FindElement(By.Id("UserName")).SendKeys(username);
                            driver.FindElement(By.Id("Password")).Click();
                            driver.FindElement(By.Id("Password")).SendKeys(Password);
                            //login click
                            driver.FindElement(By.XPath("//*[@id=\"loginForm\"]/form/div[4]/div/input")).Click();

                            Thread.Sleep(1000);
                            string currentUrlAfterLogin = driver.Url;
                            if (driver.Url == "https://localhost:44375/")
                            {
                                loginSuccess = true; // Gán true nếu đăng nhập thành công
                            }
                        }
                        //ghi ket qua ra file excel
                        worksheet.Cells[row, 3].Value = loginSuccess ? "True" : "False";

                    }
                }
                else { Console.WriteLine("Khong tim thay worksheet trong tep excel"); }
                //save vao tep excel
                excelPackage.Save();
            }
        }
        [Test]
        public void RollPageDown()
        {
            //review trang chủ (roll từ đầu xuống cuối)
            
            //cuộng từng phần xuống : 4 lần cuộn
            Actions actions = new Actions(driver);
            actions.SendKeys(Keys.PageDown).Perform();
            Thread.Sleep(500);
            Actions actions2 = new Actions(driver);
            actions2.SendKeys(Keys.PageDown).Perform();
            Thread.Sleep(500);
            Actions actions3 = new Actions(driver);
            actions3.SendKeys(Keys.PageDown).Perform();
            Thread.Sleep(500);
            Actions actions4 = new Actions(driver);
            actions4.SendKeys(Keys.PageDown).Perform();
            Thread.Sleep(500);

            //Click san pham xem detail
            IWebElement elm1 = driver.FindElement(By.XPath("/html/body/div/div[7]/div/div[2]/div/div/div[1]/div[1]/div/div[2]/div/div/div[1]/div[4]/h6/a"));
            elm1.Click();
            //Click tang sl san pham
            IWebElement elm2 = driver.FindElement(By.XPath("/html/body/div/div[3]/div[2]/div[2]/div/div[4]/div[1]/span[3]"));
            elm2.Click();
            elm2.Click();
            //add to cart
            IWebElement elm3 = driver.FindElement(By.XPath("/html/body/div/div[3]/div[2]/div[2]/div/div[4]/div[2]/a"));
            elm3.Click();
            //thông báo
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
            IAlert alert = driver.SwitchTo().Alert();
            Console.WriteLine("Alert text: Thêm sản phẩm vào giở hàng thành công!" + alert.Text);
            alert.Accept();
            
        }
        [Test]
        public void Contact()
        {
            //Module 4 contact
            //clik vào liên hệ xem ggmaps
            IWebElement elm = driver.FindElement(By.XPath("/html/body/div/header/div[2]/div/div/div/nav/ul[1]/li[3]/a"));
            elm.Click();

            //nhúng iframe ggmaps vào
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement elm1 = driver.FindElement(By.TagName("iframe"));
            driver.SwitchTo().Frame(elm1);

            //click xem bản đồ
            IWebElement elm2 = wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"mapDiv\"]/div/div[3]/div[2]\r\n")));    
            Actions actions = new Actions(driver);
            actions.MoveToElement(elm2).Click().Perform();
            //zoom
            IWebElement elm3 = driver.FindElement(By.XPath("//*[@id=\"mapDiv\"]/div/div[3]/div[3]/div\r\n"));
            elm3.Click();   

        }
        [Test]
        public void AdminLogging()
        {
            //Module5 Admin 
            //Dang nhap admin
            //tk : admin@gmail.com
            //mk : 123456
            driver.Navigate().GoToUrl("https://localhost:44375/admin/account");
            //dien tai khoan
            IWebElement elm = driver.FindElement(By.XPath("//*[@id=\"UserName\"]"));
            elm.SendKeys("admin@gmail.com");
            //dien mat khau
            IWebElement elm1 = driver.FindElement(By.XPath("//*[@id=\"Password\"]"));
            elm1.SendKeys("123456");
            //click dang nhap
            IWebElement elm2 = driver.FindElement(By.XPath("/ html / body / div / div[2] / div / form / div[3] / div[2] / button"));
            elm2.Click();

            //Click quản lí sản phẩm
            //WebDriverWait wait1 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            //IWebElement elm3 = driver.FindElement(By.XPath("/html/body/div/aside[1]/div[1]/nav/ul/li[7]/a/p"));
            //elm3.Click();

            //click sản phẩm
            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //IWebElement elm4 = driver.FindElement(By.XPath("/html/body/div/aside[1]/div[1]/nav/ul/li[7]/ul/li[2]"));
            //elm4.Click();
            ////thêm sản phẩm
            //IWebElement elm5 = driver.FindElement(By.XPath("/html/body/div[1]/div[1]/section[2]/div/div[1]/div/a[1]"));
            //elm5.Click();
            ////điền tên sản phẩm
            ////title
            //IWebElement elm6 = driver.FindElement(By.XPath("//*[@id=\"Title\"]"));
            //elm6.Click();
            ////alias
            //IWebElement elm7 = driver.FindElement(By.XPath("//*[@id=\"Alias\"]"));
            //elm7.Click();
            ////sku
            //IWebElement elm8 = driver.FindElement(By.XPath("//*[@id=\"ProductCode\"]"));
            //elm8.Click();
            ////click danh mục sản phẩm 
            //IWebElement elm9 = driver.FindElement(By.XPath("//*[@id=\"ProductCategoryId\"]"));
            //elm9.Click();
            ////click ao dai
            //IWebElement elm10 = driver.FindElement(By.XPath("//*[@id=\"ProductCategoryId\"]/option[3]"));
            //elm10.Click();
            ////dien detail sản phẩm
            //IWebElement elm11 = driver.FindElement(By.XPath("//*[@id=\"Description\"]"));
            //elm11.SendKeys("ao dep lam mua di !");

            ////click hình ảnh để thêm ảnh
            //IWebElement elm12 = driver.FindElement(By.XPath("/html/body/div/div[1]/section[2]/div/div[2]/div/div/form/div/div[1]/ul/li[2]/a"));
            //elm12.Click();

            ////click tải ảnh
            //IWebElement elm13 = driver.FindElement(By.XPath("//*[@id=\"iTaiAnh\"]"));
            //elm13.Click();
            
        }
        [TearDown]
        public void TearDown() 
        {
            //driver.Quit();



            //if (driver != null)
            //{
            //    driver.Quit();
            //    driver.Dispose();
            //    driver = null;
            //}
        }
    }
}