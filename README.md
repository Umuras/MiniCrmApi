# 🧩 MiniCRM API

Bu proje, temel bir CRM (Customer Relationship Management) sisteminin backend tarafını temsil eder.  
Amaç; müşteri, ürün ve sipariş yönetimi işlemlerini RESTful mimaride, katmanlı yapı prensiplerine uygun şekilde gerçekleştirmektir.

---

## 🚀 Teknolojiler ve Araçlar
- **.NET 8 (ASP.NET Core Web API)**
- **Entity Framework Core**
- **SQL Server (LocalDB)**
- **Dependency Injection**
- **Transaction Management**
- **Exception Middleware**
- **DTO ve AutoMapper (manuel mapping ile uygulanmıştır)**

---

## 🧱 Mimari Katmanlar
- **Controllers:** API endpoint’lerinin bulunduğu katman  
- **Services:** İş mantığının yer aldığı katman  
- **Repositories:** Veritabanı işlemlerinin soyutlandığı katman  
- **Entities (Models):** Veritabanı tablolarını temsil eder  
- **Dtos:** Veri transfer objeleri  

---

## 📦 Özellikler
- Customer, Product ve Order yönetimi
- **Order – OrderDetail** ilişkisel yapısı (1-to-many)
- Sipariş eklerken stok miktarının otomatik düşmesi
- Sipariş silindiğinde veya detay değiştiğinde **TotalPrice** ve **TotalQuantity** güncellemesi
- **Transaction** desteği ile hatalı işlemlerde otomatik rollback
- **Global Exception Middleware** ile merkezi hata yönetimi
- Enum tabanlı sipariş durumu (**Pending, Completed, Cancelled**)

---

## 🧠 Örnek Akış
1. Müşteri oluşturulur  
2. Ürün(ler) eklenir  
3. Yeni bir sipariş (Order) oluşturulurken:  
   - Ürün stokları kontrol edilir  
   - Stok miktarı azaltılır  
   - OrderDetails eklenir  
   - TotalPrice ve TotalQuantity otomatik hesaplanır  
4. Sipariş durumu (Status) güncellenebilir  

---

## 🛠 Veritabanı
Proje, **SQL Server LocalDB** üzerinde geliştirilmiştir.
