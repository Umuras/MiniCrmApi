// Entity Framework Core kütüphanesi, veritabanı işlemleri için gerekli.
using Microsoft.EntityFrameworkCore;
// Projedeki veritabanı context sınıfını ekliyoruz.
using MiniCrmApi.Data;
// Customer entity’sine erişmek için model katmanını ekliyoruz.
using MiniCrmApi.Models;

namespace MiniCrmApi.Repositories
{
    // Customer tablosu için veri erişim işlemlerini yapacak repository sınıfı.
    // ICustomerRepository arayüzünü implemente ediyor.
    public class CustomerRepository : ICustomerRepository
    {
        // Veritabanı işlemleri için EF Core context sınıfını kullanıyoruz.
        // _context üzerinden tablolara erişeceğiz.
        private readonly MiniCrmContext _context;

        // Constructor: dışarıdan MiniCrmContext nesnesini dependency injection yoluyla alır.
        // Böylece test edilebilirlik artar ve gevşek bağlılık (loosely coupled) sağlanır.
        public CustomerRepository(MiniCrmContext context)
        {
            _context = context;
        }

        // Veritabanındaki tüm Customer kayıtlarını listeler.
        // Asenkron çalışır, yani UI veya API'yi bloklamaz.
        public async Task<List<Customer>> GetAllAsync()
        {
            // ToListAsync() -> veritabanından tüm kayıtları çeker ve listeye dönüştürür.
            return await _context.Customers.ToListAsync();
        }

        // Id değerine göre tek bir Customer kaydını döndürür.
        // Ayrıca Include() ile o musteriye ait Order verilerini de birlikte ceker.
        public async Task<Customer> GetByIdAsync(int id)
        {
            // FirstOrDefaultAsync() -> kosula uyan ilk kaydi getirir, yoksa null döner.
            Customer? customer = await _context.Customers.
                Include(c => c.Orders). // Iliskili siparisleri de getir.
                FirstOrDefaultAsync(c => c.Id == id);

            // Bulunan musteriyi dondurur (null olabilir, Controller katmanında kontrol edilir).
            return customer;
        }

        // Yeni bir Customer kaydı ekler.
        public async Task AddAsync(Customer customer)
        {
            // EF Core, Customers tablosuna yeni kayıt ekleyecegimizi anlar.
            _context.Customers.Add(customer);
            // Veritabanına fiziksel olarak kaydeder.
            await _context.SaveChangesAsync();
        }

        // Mevcut bir Customer kaydını günceller.
        public async Task UpdateAsync(Customer customer)
        {
            // EF Core, verilen entity’nin degistigini isaretler.
            _context.Customers.Update(customer);
            // Degisiklikleri veritabanına yazar.
            await _context.SaveChangesAsync();
        }

        // Id’ye gore Customer kaydini siler.
        public async Task DeleteAsync(int id)
        {
            // FindAsync() -> Primary Key’e göre arama yapar (bu durumda Id).
            Customer? customer = await _context.Customers.FindAsync(id);

            // Eğer kayıt varsa silme işlemini yapar.
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            } // Eger kayıt bulunamazsa hata fırlatir.
            else
            {
                throw new Exception("There isn't customer belong to this id");
            }
        }  
    }
}
