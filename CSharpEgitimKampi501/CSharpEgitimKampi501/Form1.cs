using CSharpEgitimKampi501.Dtos;
using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpEgitimKampi501
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection connection = new SqlConnection("Server=OZAN;initial catalog=EgitimKampi501Db;integrated security=true");

        private async void btnList_Click(object sender, EventArgs e)
        {
            string query = "Select * from TblProduct";
            var values = await connection.QueryAsync<ResultProductDto>(query); //QueryAsync metodu dapper kütüphanesinin metodu olup, veritabanından veri çekmek için kullanılır.
            dataGridView1.DataSource = values;
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            string query = "Insert into TblProduct (ProductName, ProductStock ,ProductPrice, ProductCategory) values (@name, @stock ,@price,@category)";
            var parameters = new DynamicParameters(); //Dapper kütüphanesinin metodu olup, parametre eklemek için kullanılır.
            parameters.Add("@name", txtProductName.Text);
            parameters.Add("@stock", txtProductStock.Text);
            parameters.Add("@price", txtProductPrice.Text);
            parameters.Add("@category", txtProductCategory.Text);
            await connection.ExecuteAsync(query, parameters); //ExecuteAsync metodu dapper kütüphanesinin metodu olup, veritabanına veri eklemek için kullanılır.
            MessageBox.Show("Kitap Başarıyla Eklendi.");
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            string query = "Delete from TblProduct where ProductId = @id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", txtProductId.Text);
            await connection.ExecuteAsync(query, parameters);
            MessageBox.Show("Kitap Başarıyla Silindi.");
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            string query = "Update TblProduct Set ProductName=@productName, ProductPrice=@productPrice, ProductStock=@productStock," +
                "ProductCategory=@productCategory where ProductId=@productId";
            var parameters = new DynamicParameters();
            parameters.Add("@productName", txtProductName.Text);
            parameters.Add("@productPrice", txtProductPrice.Text);
            parameters.Add("@productStock", txtProductStock.Text);
            parameters.Add("@productCategory", txtProductCategory.Text);
            parameters.Add("@productId", txtProductId.Text);
            await connection.ExecuteAsync(query, parameters);
            MessageBox.Show("Kitap Başarıyla Güncellendi.","Güncelleme",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            string query1 = "Select Count(*) from TblProduct";
            var productTotalCount = await connection.QueryAsync<int>(query1); //<int> türünde bir liste döndürür.
            lblTotalProductCount.Text = productTotalCount.FirstOrDefault().ToString();

            string query2 = "Select ProductName from TblProduct Where ProductPrice = (Select MAX(ProductPrice) from TblProduct)";
            var maxPriceProduct = await connection.QueryAsync<string>(query2);
            lblMaxPriceBooksName.Text = maxPriceProduct.FirstOrDefault().ToString();

            string query3 = "Select Count(DISTINCT ProductCategory) from TblProduct";
            var categoryCount = await connection.QueryAsync<int>(query3);
            lblBookCategoryCount.Text = categoryCount.FirstOrDefault().ToString();
        }
    }
}
