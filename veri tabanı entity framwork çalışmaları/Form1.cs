using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;  //ders listesi komutu için kütüphane ekledik.

namespace veri_tabanı_entity_framwork_çalışmaları
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //ogrenci entities bizim modelimiz bu modeli oluşturarak db adında bir nesne ürettik ve tablo görüntülemesi yaptık.
        DbsınavOgrenciEntities db = new DbsınavOgrenciEntities();
         void ogrenci_listele()
        {
            dataGridView1.DataSource = db.TBLOGRENCİ.ToList();

            /*buradaki poblem ogrenci listele dediğimizde tblogrenci tablosu çekiliyor ve bu tabloda tblnotlar kısmı ile ilişkili olduğu 
             için veriyi göstermeye çalışınca tbl notlar kısmıda gözüküyor.. (ilişkili olduğu tabloda çıkıyor)*/
            dataGridView1.Columns[3].Visible = false; //foto kısmıda gelmesin
            dataGridView1.Columns[4].Visible = false; //bu şekil yaparak 4. sütunu yazmayı engelleriz.
        }

        void ders_listele()
        {
            SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-E88VK6V\SQLEXPRESS;Initial Catalog=DbsınavOgrenci;Integrated Security=True");
            SqlCommand komut = new SqlCommand("Select *From tblders ", baglanti);

            baglanti.Open();

            SqlDataAdapter da = new SqlDataAdapter(komut); //bu yapıya bak
            DataTable dt = new DataTable();  //verileri tuttuğumuz alan 
            da.Fill(dt);
            dataGridView1.DataSource = dt; //verileri ekrana göstermek adına dataGridView kullandık
            baglanti.Close();              //dataGridViewin gösterme komutu ise datasource'dür..


        }
        private void BtnDersListesi_Click(object sender, EventArgs e) //ders listesini çağırmak için sql kullandık(farkı görebilmek)
        {
            ders_listele();
        }


        private void Ogrenci_Enter(object sender, EventArgs e)
        {

        }

        private void BtnOgrenciListele_Click(object sender, EventArgs e) //entity framework ile oluşturma
        {
            ogrenci_listele();
        }

        private void BtnNotListesi_Click(object sender, EventArgs e)
        {
            //dataGridView1.DataSource = db.TBLNOTLAR.ToList();  //(normal gösterim)

            //tblnotlar tablosundaki bazı alanları seçerek koyduk tüm alanlar yerine seçilen alaları yazdırmak için bu yontem var.
            var query = from item in db.TBLNOTLAR   /*veri listeleme yaparken tablo bazlı birleştirmede yapabiliriz aşağıda ki ornekteki gibi not listesini çekerken aynı zamanda
                                                     ogrenci tablosundan ad soyad ve aynı zamanda ders tablosundan dersadı getiriyoruz. */
                        select new { item.NOTID, item.TBLOGRENCİ.AD, item.TBLOGRENCİ.SOYAD, item.TBLDERS.DERSAD, item.OGR, item.DERS, item.SINAV1, item.SINAV2, item.SINAV3, item.ORTALAMA, item.DURUM };
            dataGridView1.DataSource = query.ToList();


        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            TBLOGRENCİ t = new TBLOGRENCİ();
            if(TxtAd.Text!=string.Empty && TxtSoyad.Text!=string.Empty )
            {
                t.AD = TxtAd.Text;
                t.SOYAD = TxtSoyad.Text;
                db.TBLOGRENCİ.Add(t); //ogrenci tablosundaki verileri akle komutudur (entity framework)
                db.SaveChanges(); //en son yapılan ifadeyi kaydet anlamı taşır.. (executenonquary gibi)
                MessageBox.Show("Öğrenci listeye eklenmiştir");
                ogrenci_listele();

            }
         

            if(TxtDers.Text!=string.Empty)
            {
                //bu kısma ders ekleme ozelliği yaz
                TBLDERS d = new TBLDERS();
                d.DERSAD = TxtDers.Text;
                db.TBLDERS.Add(d);  //tblders veri tabanına ekle komutudur.
                db.SaveChanges();
                MessageBox.Show("ders listeye eklenmiştir");
                ders_listele();
            }
          


        }

        private void BtnSil_Click(object sender, EventArgs e)  //silme komutu 
        {
            int id = Convert.ToInt32(TxtOgrenciId.Text); //öğrenci silme komutu
            var x = db.TBLOGRENCİ.Find(id);
            db.TBLOGRENCİ.Remove(x); //oğrenci sildiğimiz için tblogrenci tablosundan silinecek tıpkı diğer işlemler gibi
            db.SaveChanges();
            MessageBox.Show("ogrenci kayittan silindi");
        }

        private void BtnGuncelle_Click(object sender, EventArgs e) //güncelleme butonu
        {
            int id = Convert.ToInt32(TxtOgrenciId.Text); //güncelleme işlemi
            var x = db.TBLOGRENCİ.Find(id);
            //güncelleme işleminin bir metodu yok bunun yerine atama işlemi yapıyoruz
            x.AD = TxtAd.Text;
            x.SOYAD = TxtSoyad.Text;
            x.FOTOGRAF = TxtFotograf.Text;
            db.SaveChanges();
            MessageBox.Show("ogrenci kayıdınız güncellendi");
            ogrenci_listele();

        }



        private void ButtonJoin_Click(object sender, EventArgs e) //join komutu ile tablo birleştirme not listesi butonundaki 
        {                                                         // birleştirmenin aynısını join komutu ile yapacağız.
            var sorgu = from d1 in db.TBLNOTLAR
                        join d2 in db.TBLOGRENCİ
                        on d1.OGR equals d2.ID
                        join d3 in db.TBLDERS
                        on d1.DERS equals d3.DERSID
                        select new
                        {
                            ÖĞRENCİ= d2.AD+" "+d2.SOYAD,
                            DERS=d3.DERSAD,
                            SINAV1= d1.SINAV1,
                            SINAV2=d1.SINAV2,
                            SINAV3=d1.SINAV3,
                            ORTALAMA=d1.ORTALAMA
                            



                        };
            dataGridView1.DataSource = sorgu.ToList();

        }



        private void BtnProsedur_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.NOTLISTESI();

        }

        private void BtnBul_Click(object sender, EventArgs e)  //bul komutu belirli bir kullanıcı değerine göre veri çekme

        { //lambda sorgulama ifadeleri bu sorguya göre tabloda ogrenci verisi yüklenir
            dataGridView1.DataSource = db.TBLOGRENCİ.Where(x => x.AD == TxtAd.Text && x.SOYAD == TxtSoyad.Text).ToList();
        }




        private void TxtAd_TextChanged(object sender, EventArgs e) //ad txt'si 
        { //burada yazılan komutlar ad kısmında aranan ifadeleri net şekilde çıkaran ifadelerdir 
            //ornek al diye ad kısmında kelime aratsın içinde al olan ifadeler çıkar.

            string aranan_ifade = TxtAd.Text;

            var degerler = from item in db.TBLOGRENCİ
                           where item.AD.Contains(aranan_ifade) 
                           select item;

            dataGridView1.DataSource = degerler.ToList();
                           

        }
        private void BtnLinqEntity_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked==true)
            {    //orderby ascending sıralaması ismi a'dan z'ye sıralıyor.
                List<TBLOGRENCİ> liste1 = db.TBLOGRENCİ.OrderBy(p => p.AD).ToList();
                dataGridView1.DataSource = liste1;
            }

            if(radioButton2.Checked==true)
            {   //order by descending sıralaması ismi z'den a'ya sıralaması
                List<TBLOGRENCİ> liste2 = db.TBLOGRENCİ.OrderByDescending(p => p.AD).ToList();
                dataGridView1.DataSource = liste2;
            }

            if (radioButton3.Checked == true)
            {   //order by ile ilk 3 tanesini al (a'dan z'ye)
                List<TBLOGRENCİ> liste3 = db.TBLOGRENCİ.OrderBy(p => p.AD).Take(3).ToList();
                dataGridView1.DataSource = liste3;
            }

            if (radioButton4.Checked == true)
            {   //ismi a ile başlayanları getir
                List<TBLOGRENCİ> liste4 = db.TBLOGRENCİ.Where(p => p.AD.StartsWith("a")).ToList();
                dataGridView1.DataSource = liste4;
            }

            if(radioButton5.Checked==true) //toplam ogrenci sayısını getirsin
            {
                int toplam = db.TBLOGRENCİ.Count();
                MessageBox.Show(toplam.ToString(), "TOPLAM OGRENCİ SAYISI", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                                                   //messagebox sırası 1. bilgi 2. button.ok 3. ise information                  

            }

            if (radioButton6.Checked == true) // 1. sınavın ortalamasını versin 
            {
                var ortalama = db.TBLNOTLAR.Average(p => p.SINAV1);  //var dememizin sebebi int tanımlamasında hata veriyor
                MessageBox.Show("birinci sınavın ortalaması =  "+ ortalama.ToString());
                               

            }

            if(radioButton7.Checked==true) //1. sınav notu ortalamadan yüksek olanların listelenmesi
            {

                var ortalama = db.TBLNOTLAR.Average(p => p.SINAV1);

                List<TBLNOTLAR> liste = db.TBLNOTLAR.Where(p => p.SINAV1 > ortalama).ToList();
                dataGridView1.DataSource = liste;
            }


            /* diğer linnq sorgu ornekleri:
               toplam der ise  Sum komutu ile bunu yaparız.
               en yüksek ifadesş max komutu ile olur en düşük min ile olur*/

        }

        private void BtnHesapla_Click(object sender, EventArgs e)
        {

        }

        private void BtnNotGuncelle_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        
    }
}
