using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsTakipWpf.Models;

namespace IsTakipWpf.Services
{
    public class LocationService : ILocationService
    {
        private readonly List<City> _cities;

        public LocationService()
        {
            _cities = GenerateData();
        }

        public Task<IEnumerable<City>> GetCitiesAsync()
        {
            return Task.FromResult(_cities.AsEnumerable());
        }

        public Task<IEnumerable<string>> GetDistrictsAsync(string cityName)
        {
            var city = _cities.FirstOrDefault(c => c.Name == cityName);
            if (city == null) return Task.FromResult(Enumerable.Empty<string>());
            return Task.FromResult(city.Districts.AsEnumerable());
        }

        private List<City> GenerateData()
        {
            var list = new List<City>();
            
            // Adana
            list.Add(new City { Id = 1, Name = "Adana", Districts = new List<string> { "Aladağ", "Ceyhan", "Çukurova", "Feke", "İmamoğlu", "Karaisalı", "Karataş", "Kozan", "Pozantı", "Saimbeyli", "Sarıçam", "Seyhan", "Tufanbeyli", "Yumurtalık", "Yüreğir" } });
            // Adıyaman
            list.Add(new City { Id = 2, Name = "Adıyaman", Districts = new List<string> { "Besni", "Çelikhan", "Gerger", "Gölbaşı", "Kahta", "Merkez", "Samsat", "Sincik", "Tut" } });
            // Afyonkarahisar
            list.Add(new City { Id = 3, Name = "Afyonkarahisar", Districts = new List<string> { "Başmakçı", "Bayat", "Bolvadin", "Çay", "Çobanlar", "Dazkırı", "Dinar", "Emirdağ", "Evciler", "Hocalar", "İhsaniye", "İscehisar", "Kızılören", "Merkez", "Sandıklı", "Sinanpaşa", "Sultandağı", "Şuhut" } });
            // Ağrı
            list.Add(new City { Id = 4, Name = "Ağrı", Districts = new List<string> { "Diyadin", "Doğubayazıt", "Eleşkirt", "Hamur", "Merkez", "Patnos", "Taşlıçay", "Tutak" } });
            // Amasya
            list.Add(new City { Id = 5, Name = "Amasya", Districts = new List<string> { "Göynücek", "Gümüşhacıköy", "Hamamözü", "Merkez", "Merzifon", "Suluova", "Taşova" } });
            // Ankara
            list.Add(new City { Id = 6, Name = "Ankara", Districts = new List<string> { "Akyurt", "Altındağ", "Ayaş", "Bala", "Beypazarı", "Çamlıdere", "Çankaya", "Çubuk", "Elmadağ", "Etimesgut", "Evren", "Gölbaşı", "Güdül", "Haymana", "Kalecik", "Kahramankazan", "Keçiören", "Kızılcahamam", "Mamak", "Nallıhan", "Polatlı", "Pursaklar", "Sincan", "Şereflikoçhisar", "Yenimahalle" } });
            // Antalya
            list.Add(new City { Id = 7, Name = "Antalya", Districts = new List<string> { "Akseki", "Aksu", "Alanya", "Demre", "Döşemealtı", "Elmalı", "Finike", "Gazipaşa", "Gündoğmuş", "İbradı", "Kaş", "Kemer", "Kepez", "Konyaaltı", "Korkuteli", "Kumluca", "Manavgat", "Muratpaşa", "Serik" } });
            // Artvin
            list.Add(new City { Id = 8, Name = "Artvin", Districts = new List<string> { "Ardanuç", "Arhavi", "Borçka", "Hopa", "Kemalpaşa", "Merkez", "Murgul", "Şavşat", "Yusufeli" } });
            // Aydın
            list.Add(new City { Id = 9, Name = "Aydın", Districts = new List<string> { "Bozdoğan", "Buharkent", "Çine", "Didim", "Efeler", "Germencik", "İncirliova", "Karacasu", "Karpuzlu", "Koçarlı", "Köşk", "Kuşadası", "Kuyucak", "Nazilli", "Söke", "Sultanhisar", "Yenipazar" } });
            // Balıkesir
            list.Add(new City { Id = 10, Name = "Balıkesir", Districts = new List<string> { "Altıeylül", "Ayvalık", "Balya", "Bandırma", "Bigadiç", "Burhaniye", "Dursunbey", "Edremit", "Erdek", "Gömeç", "Gönen", "Havran", "İvrindi", "Karesi", "Kepsut", "Manyas", "Marmara", "Savaştepe", "Sındırgı", "Susurluk" } });
            // Bilecik
            list.Add(new City { Id = 11, Name = "Bilecik", Districts = new List<string> { "Bozüyük", "Gölpazarı", "İnhisar", "Merkez", "Osmaneli", "Pazaryeri", "Söğüt", "Yenipazar" } });
            // Bingöl
            list.Add(new City { Id = 12, Name = "Bingöl", Districts = new List<string> { "Adaklı", "Genç", "Karlıova", "Kiğı", "Merkez", "Solhan", "Yayladere", "Yedisu" } });
            // Bitlis
            list.Add(new City { Id = 13, Name = "Bitlis", Districts = new List<string> { "Adilcevaz", "Ahlat", "Güroymak", "Hizan", "Merkez", "Mutki", "Tatvan" } });
            // Bolu
            list.Add(new City { Id = 14, Name = "Bolu", Districts = new List<string> { "Dörtdivan", "Gerede", "Göynük", "Kıbrıscık", "Mengen", "Merkez", "Mudurnu", "Seben", "Yeniçağa" } });
            // Burdur
            list.Add(new City { Id = 15, Name = "Burdur", Districts = new List<string> { "Ağlasun", "Altınyayla", "Bucak", "Çavdır", "Çeltikçi", "Gölhisar", "Karamanlı", "Kemer", "Merkez", "Tefenni", "Yeşilova" } });
            // Bursa
            list.Add(new City { Id = 16, Name = "Bursa", Districts = new List<string> { "Büyükorhan", "Gemlik", "Gürsu", "Harmancık", "İnegöl", "İznik", "Karacabey", "Keles", "Kestel", "Mudanya", "Mustafakemalpaşa", "Nilüfer", "Orhaneli", "Orhangazi", "Osmangazi", "Yenişehir", "Yıldırım" } });
            // Çanakkale
            list.Add(new City { Id = 17, Name = "Çanakkale", Districts = new List<string> { "Ayvacık", "Bayramiç", "Biga", "Bozcaada", "Çan", "Eceabat", "Ezine", "Gelibolu", "Gökçeada", "Lapseki", "Merkez", "Yenice" } });
            // Çankırı
            list.Add(new City { Id = 18, Name = "Çankırı", Districts = new List<string> { "Atkaracalar", "Bayramören", "Çerkeş", "Eldivan", "Ilgaz", "Kızılırmak", "Korgun", "Kurşunlu", "Merkez", "Orta", "Şabanözü", "Yapraklı" } });
            // Çorum
            list.Add(new City { Id = 19, Name = "Çorum", Districts = new List<string> { "Alaca", "Bayat", "Boğazkale", "Dodurga", "İskilip", "Kargı", "Laçin", "Mecitözü", "Merkez", "Oğuzlar", "Ortaköy", "Osmancık", "Sungurlu", "Uğurludağ" } });
            // Denizli
            list.Add(new City { Id = 20, Name = "Denizli", Districts = new List<string> { "Acıpayam", "Babadağ", "Baklan", "Bekilli", "Beyağaç", "Bozkurt", "Buldan", "Çal", "Çameli", "Çardak", "Çivril", "Güney", "Honaz", "Kale", "Merkezefendi", "Pamukkale", "Sarayköy", "Serinhisar", "Tavas" } });
            // Diyarbakır
            list.Add(new City { Id = 21, Name = "Diyarbakır", Districts = new List<string> { "Bağlar", "Bismil", "Çermik", "Çınar", "Çüngüş", "Dicle", "Eğil", "Ergani", "Hani", "Hazro", "Kayapınar", "Kocaköy", "Kulp", "Lice", "Silvan", "Sur", "Yenişehir" } });
            // Edirne
            list.Add(new City { Id = 22, Name = "Edirne", Districts = new List<string> { "Enez", "Havsa", "İpsala", "Keşan", "Lalapaşa", "Meriç", "Merkez", "Süloğlu", "Uzunköprü" } });
            // Elazığ
            list.Add(new City { Id = 23, Name = "Elazığ", Districts = new List<string> { "Ağın", "Alacakaya", "Arıcak", "Baskil", "Karakoçan", "Keban", "Kovancılar", "Maden", "Merkez", "Palu", "Sivrice" } });
            // Erzincan
            list.Add(new City { Id = 24, Name = "Erzincan", Districts = new List<string> { "Çayırlı", "İliç", "Kemah", "Kemaliye", "Merkez", "Otlukbeli", "Refahiye", "Tercan", "Üzümlü" } });
            // Erzurum
            list.Add(new City { Id = 25, Name = "Erzurum", Districts = new List<string> { "Aşkale", "Aziziye", "Çat", "Hınıs", "Horasan", "İspir", "Karaçoban", "Karayazı", "Köprüköy", "Narman", "Oltu", "Olur", "Palandöken", "Pasinler", "Pazaryolu", "Şenkaya", "Tekman", "Tortum", "Uzundere", "Yakutiye" } });
            // Eskişehir
            list.Add(new City { Id = 26, Name = "Eskişehir", Districts = new List<string> { "Alpu", "Beylikova", "Çifteler", "Günyüzü", "Han", "İnönü", "Mahmudiye", "Mihalgazi", "Mihalıççık", "Odunpazarı", "Sarıcakaya", "Seyitgazi", "Sivrihisar", "Tepebaşı" } });
            // Gaziantep
            list.Add(new City { Id = 27, Name = "Gaziantep", Districts = new List<string> { "Araban", "İslahiye", "Karkamış", "Nizip", "Nurdağı", "Oğuzeli", "Şahinbey", "Şehitkamil", "Yavuzeli" } });
            // Giresun
            list.Add(new City { Id = 28, Name = "Giresun", Districts = new List<string> { "Alucra", "Bulancak", "Çamoluk", "Çanakçı", "Dereli", "Doğankent", "Espiye", "Eynesil", "Görele", "Güce", "Keşap", "Merkez", "Piraziz", "Şebinkarahisar", "Tirebolu", "Yağlıdere" } });
            // Gümüşhane
            list.Add(new City { Id = 29, Name = "Gümüşhane", Districts = new List<string> { "Kelkit", "Köse", "Kürtün", "Merkez", "Şiran", "Torul" } });
            // Hakkari
            list.Add(new City { Id = 30, Name = "Hakkari", Districts = new List<string> { "Çukurca", "Derecik", "Merkez", "Şemdinli", "Yüksekova" } });
            // Hatay
            list.Add(new City { Id = 31, Name = "Hatay", Districts = new List<string> { "Altınözü", "Antakya", "Arsuz", "Belen", "Defne", "Dörtyol", "Erzin", "Hassa", "İskenderun", "Kırıkhan", "Kumlu", "Payas", "Reyhanlı", "Samandağ", "Yayladağı" } });
            // Isparta
            list.Add(new City { Id = 32, Name = "Isparta", Districts = new List<string> { "Aksu", "Atabey", "Eğirdir", "Gelendost", "Gönen", "Keçiborlu", "Merkez", "Senirkent", "Sütçüler", "Şarkikaraağaç", "Uluborlu", "Yalvaç", "Yenişarbademli" } });
            // Mersin
            list.Add(new City { Id = 33, Name = "Mersin", Districts = new List<string> { "Akdeniz", "Anamur", "Aydıncık", "Bozyazı", "Çamlıyayla", "Erdemli", "Gülnar", "Mezitli", "Mut", "Silifke", "Tarsus", "Toroslar", "Yenişehir" } });
            // İstanbul
            list.Add(new City { Id = 34, Name = "İstanbul", Districts = new List<string> { "Adalar", "Arnavutköy", "Ataşehir", "Avcılar", "Bağcılar", "Bahçelievler", "Bakırköy", "Başakşehir", "Bayrampaşa", "Beşiktaş", "Beykoz", "Beylikdüzü", "Beyoğlu", "Büyükçekmece", "Çatalca", "Çekmeköy", "Esenler", "Esenyurt", "Eyüpsultan", "Fatih", "Gaziosmanpaşa", "Güngören", "Kadıköy", "Kağıthane", "Kartal", "Küçükçekmece", "Maltepe", "Pendik", "Sancaktepe", "Sarıyer", "Silivri", "Sultanbeyli", "Sultangazi", "Şile", "Şişli", "Tuzla", "Ümraniye", "Üsküdar", "Zeytinburnu" } });
            // İzmir
            list.Add(new City { Id = 35, Name = "İzmir", Districts = new List<string> { "Aliağa", "Balçova", "Bayındır", "Bayraklı", "Bergama", "Beydağ", "Bornova", "Buca", "Çeşme", "Çiğli", "Dikili", "Foça", "Gaziemir", "Güzelbahçe", "Karabağlar", "Karaburun", "Karşıyaka", "Kemalpaşa", "Kınık", "Kiraz", "Konak", "Menderes", "Menemen", "Narlıdere", "Ödemiş", "Seferihisar", "Selçuk", "Tire", "Torbalı", "Urla" } });
            // Kars
            list.Add(new City { Id = 36, Name = "Kars", Districts = new List<string> { "Akyaka", "Arpaçay", "Digor", "Kağızman", "Merkez", "Sarıkamış", "Selim", "Susuz" } });
            // Kastamonu
            list.Add(new City { Id = 37, Name = "Kastamonu", Districts = new List<string> { "Abana", "Ağlı", "Araç", "Azdavay", "Bozkurt", "Cide", "Çatalzeytin", "Daday", "Devrekani", "Doğanyurt", "Hanönü", "İhsangazi", "İnebolu", "Küre", "Merkez", "Pınarbaşı", "Seydiler", "Şenpazar", "Taşköprü", "Tosya" } });
            // Kayseri
            list.Add(new City { Id = 38, Name = "Kayseri", Districts = new List<string> { "Akkışla", "Bünyan", "Develi", "Felahiye", "Hacılar", "İncesu", "Kocasinan", "Melikgazi", "Özvatan", "Pınarbaşı", "Sarıoğlan", "Sarız", "Talas", "Tomarza", "Yahyalı", "Yeşilhisar" } });
            // Kırklareli
            list.Add(new City { Id = 39, Name = "Kırklareli", Districts = new List<string> { "Babaeski", "Demirköy", "Kofçaz", "Lüleburgaz", "Merkez", "Pehlivanköy", "Pınarhisar", "Vize" } });
            // Kırşehir
            list.Add(new City { Id = 40, Name = "Kırşehir", Districts = new List<string> { "Akçakent", "Akpınar", "Boztepe", "Çiçekdağı", "Kaman", "Merkez", "Mucur" } });
            // Kocaeli
            list.Add(new City { Id = 41, Name = "Kocaeli", Districts = new List<string> { "Başiskele", "Çayırova", "Darıca", "Derince", "Dilovası", "Gebze", "Gölcük", "İzmit", "Kandıra", "Karamürsel", "Kartepe", "Körfez" } });
            // Konya
            list.Add(new City { Id = 42, Name = "Konya", Districts = new List<string> { "Ahırlı", "Akören", "Akşehir", "Altınekin", "Beyşehir", "Bozkır", "Cihanbeyli", "Çeltik", "Çumra", "Derbent", "Derebucak", "Doğanhisar", "Emirgazi", "Ereğli", "Güneysınır", "Hadim", "Halkapınar", "Hüyük", "Ilgın", "Kadınhanı", "Karapınar", "Karatay", "Kulu", "Meram", "Sarayönü", "Selçuklu", "Seydişehir", "Taşkent", "Tuzlukçu", "Yalıhüyük", "Yunak" } });
            // Kütahya
            list.Add(new City { Id = 43, Name = "Kütahya", Districts = new List<string> { "Altıntaş", "Aslanapa", "Çavdarhisar", "Domaniç", "Dumlupınar", "Emet", "Gediz", "Hisarcık", "Merkez", "Pazarlar", "Simav", "Şaphane", "Tavşanlı" } });
            // Malatya
            list.Add(new City { Id = 44, Name = "Malatya", Districts = new List<string> { "Akçadağ", "Arapgir", "Arguvan", "Battalgazi", "Darende", "Doğanşehir", "Doğanyol", "Hekimhan", "Kale", "Kuluncak", "Pütürge", "Yazıhan", "Yeşilyurt" } });
            // Manisa
            list.Add(new City { Id = 45, Name = "Manisa", Districts = new List<string> { "Ahmetli", "Akhisar", "Alaşehir", "Demirci", "Gölmarmara", "Gördes", "Kırkağaç", "Köprübaşı", "Kula", "Salihli", "Sarıgöl", "Saruhanlı", "Selendi", "Soma", "Şehzadeler", "Turgutlu", "Yunusemre" } });
            // Kahramanmaraş
            list.Add(new City { Id = 46, Name = "Kahramanmaraş", Districts = new List<string> { "Afşin", "Andırın", "Çağlayancerit", "Dulkadiroğlu", "Ekinözü", "Elbistan", "Göksun", "Nurhak", "Onikişubat", "Pazarcık", "Türkoğlu" } });
            // Mardin
            list.Add(new City { Id = 47, Name = "Mardin", Districts = new List<string> { "Artuklu", "Dargeçit", "Derik", "Kızıltepe", "Mazıdağı", "Midyat", "Nusaybin", "Ömerli", "Savur", "Yeşilli" } });
            // Muğla
            list.Add(new City { Id = 48, Name = "Muğla", Districts = new List<string> { "Bodrum", "Dalaman", "Datça", "Fethiye", "Kavaklıdere", "Köyceğiz", "Marmaris", "Menteşe", "Milas", "Ortaca", "Seydikemer", "Ula", "Yatağan" } });
            // Muş
            list.Add(new City { Id = 49, Name = "Muş", Districts = new List<string> { "Bulanık", "Hasköy", "Korkut", "Malazgirt", "Merkez", "Varto" } });
            // Nevşehir
            list.Add(new City { Id = 50, Name = "Nevşehir", Districts = new List<string> { "Acıgöl", "Avanos", "Derinkuyu", "Gülşehir", "Hacıbektaş", "Kozaklı", "Merkez", "Ürgüp" } });
            // Niğde
            list.Add(new City { Id = 51, Name = "Niğde", Districts = new List<string> { "Altunhisar", "Bor", "Çamardı", "Çiftlik", "Merkez", "Ulukışla" } });
            // Ordu
            list.Add(new City { Id = 52, Name = "Ordu", Districts = new List<string> { "Akkuş", "Altınordu", "Aybastı", "Çamaş", "Çatalpınar", "Çaybaşı", "Fatsa", "Gölköy", "Gülyalı", "Gürgentepe", "İkizce", "Kabadüz", "Kabataş", "Korgan", "Kumru", "Mesudiye", "Perşembe", "Ulubey", "Ünye" } });
            // Rize
            list.Add(new City { Id = 53, Name = "Rize", Districts = new List<string> { "Ardeşen", "Çamlıhemşin", "Çayeli", "Derepazarı", "Fındıklı", "Güneysu", "Hemşin", "İkizdere", "İyidere", "Kalkandere", "Merkez", "Pazar" } });
            // Sakarya
            list.Add(new City { Id = 54, Name = "Sakarya", Districts = new List<string> { "Adapazarı", "Akyazı", "Arifiye", "Erenler", "Ferizli", "Geyve", "Hendek", "Karapürçek", "Karasu", "Kaynarca", "Kocaali", "Pamukova", "Sapanca", "Serdivan", "Söğütlü", "Taraklı" } });
            // Samsun
            list.Add(new City { Id = 55, Name = "Samsun", Districts = new List<string> { "Alaçam", "Asarcık", "Atakum", "Ayvacık", "Bafra", "Canik", "Çarşamba", "Havza", "İlkadım", "Kavak", "Ladik", "Salıpazarı", "Tekkeköy", "Terme", "Vezirköprü", "Yakakent" } });
            // Siirt
            list.Add(new City { Id = 56, Name = "Siirt", Districts = new List<string> { "Baykan", "Eruh", "Kurtalan", "Merkez", "Pervari", "Şirvan", "Tillo" } });
            // Sinop
            list.Add(new City { Id = 57, Name = "Sinop", Districts = new List<string> { "Ayancık", "Boyabat", "Dikmen", "Durağan", "Erfelek", "Gerze", "Merkez", "Saraydüzü", "Türkeli" } });
            // Sivas
            list.Add(new City { Id = 58, Name = "Sivas", Districts = new List<string> { "Akıncılar", "Altınyayla", "Divriği", "Doğanşar", "Gemerek", "Gölova", "Gürün", "Hafik", "İmranlı", "Kangal", "Koyulhisar", "Merkez", "Suşehri", "Şarkışla", "Ulaş", "Yıldızeli", "Zara" } });
            // Tekirdağ
            list.Add(new City { Id = 59, Name = "Tekirdağ", Districts = new List<string> { "Çerkezköy", "Çorlu", "Ergene", "Hayrabolu", "Kapaklı", "Malkara", "Marmaraereğlisi", "Muratlı", "Saray", "Süleymanpaşa", "Şarköy" } });
            // Tokat
            list.Add(new City { Id = 60, Name = "Tokat", Districts = new List<string> { "Almus", "Artova", "Başçiftlik", "Erbaa", "Merkez", "Niksar", "Pazar", "Reşadiye", "Sulusaray", "Turhal", "Yeşilyurt", "Zile" } });
            // Trabzon
            list.Add(new City { Id = 61, Name = "Trabzon", Districts = new List<string> { "Akçaabat", "Araklı", "Arsin", "Beşikdüzü", "Çarşıbaşı", "Çaykara", "Dernekpazarı", "Düzköy", "Hayrat", "Köprübaşı", "Maçka", "Of", "Ortahisar", "Sürmene", "Şalpazarı", "Tonya", "Vakfıkebir", "Yomra" } });
            // Tunceli
            list.Add(new City { Id = 62, Name = "Tunceli", Districts = new List<string> { "Çemişgezek", "Hozat", "Mazgirt", "Merkez", "Nazımiye", "Ovacık", "Pertek", "Pülümür" } });
            // Şanlıurfa
            list.Add(new City { Id = 63, Name = "Şanlıurfa", Districts = new List<string> { "Akçakale", "Birecik", "Bozova", "Ceylanpınar", "Eyyübiye", "Halfeti", "Haliliye", "Harran", "Hilvan", "Karaköprü", "Siverek", "Suruç", "Viranşehir" } });
            // Uşak
            list.Add(new City { Id = 64, Name = "Uşak", Districts = new List<string> { "Banaz", "Eşme", "Karahallı", "Merkez", "Sivaslı", "Ulubey" } });
            // Van
            list.Add(new City { Id = 65, Name = "Van", Districts = new List<string> { "Bahçesaray", "Başkale", "Çaldıran", "Çatak", "Edremit", "Erciş", "Gevaş", "Gürpınar", "İpekyolu", "Muradiye", "Özalp", "Saray", "Tuşba" } });
            // Yozgat
            list.Add(new City { Id = 66, Name = "Yozgat", Districts = new List<string> { "Akdağmadeni", "Aydıncık", "Boğazlıyan", "Çandır", "Çayıralan", "Çekerek", "Kadışehri", "Merkez", "Saraykent", "Sarıkaya", "Sorgun", "Şefaatli", "Yenifakılı", "Yerköy" } });
            // Zonguldak
            list.Add(new City { Id = 67, Name = "Zonguldak", Districts = new List<string> { "Alaplı", "Çaycuma", "Devrek", "Ereğli", "Gökçebey", "Kilimli", "Kozlu", "Merkez" } });
            // Aksaray
            list.Add(new City { Id = 68, Name = "Aksaray", Districts = new List<string> { "Ağaçören", "Eskil", "Gülağaç", "Güzelyurt", "Merkez", "Ortaköy", "Sarıyahşi", "Sultanhanı" } });
            // Bayburt
            list.Add(new City { Id = 69, Name = "Bayburt", Districts = new List<string> { "Aydıntepe", "Demirözü", "Merkez" } });
            // Karaman
            list.Add(new City { Id = 70, Name = "Karaman", Districts = new List<string> { "Ayrancı", "Başyayla", "Ermenek", "Kazımkarabekir", "Merkez", "Sarıveliler" } });
            // Kırıkkale
            list.Add(new City { Id = 71, Name = "Kırıkkale", Districts = new List<string> { "Bahşılı", "Balışeyh", "Çelebi", "Delice", "Karakeçili", "Keskin", "Merkez", "Sulakyurt", "Yahşihan" } });
            // Batman
            list.Add(new City { Id = 72, Name = "Batman", Districts = new List<string> { "Beşiri", "Gercüş", "Hasankeyf", "Kozluk", "Merkez", "Sason" } });
            // Şırnak
            list.Add(new City { Id = 73, Name = "Şırnak", Districts = new List<string> { "Beytüşşebap", "Cizre", "Güçlükonak", "İdil", "Merkez", "Silopi", "Uludere" } });
            // Bartın
            list.Add(new City { Id = 74, Name = "Bartın", Districts = new List<string> { "Amasra", "Kurucaşile", "Merkez", "Ulus" } });
            // Ardahan
            list.Add(new City { Id = 75, Name = "Ardahan", Districts = new List<string> { "Çıldır", "Damal", "Göle", "Hanak", "Merkez", "Posof" } });
            // Iğdır
            list.Add(new City { Id = 76, Name = "Iğdır", Districts = new List<string> { "Aralık", "Karakoyunlu", "Merkez", "Tuzluca" } });
            // Yalova
            list.Add(new City { Id = 77, Name = "Yalova", Districts = new List<string> { "Altınova", "Armutlu", "Çınarcık", "Çiftlikköy", "Merkez", "Termal" } });
            // Karabük
            list.Add(new City { Id = 78, Name = "Karabük", Districts = new List<string> { "Eflani", "Eskipazar", "Merkez", "Ovacık", "Safranbolu", "Yenice" } });
            // Kilis
            list.Add(new City { Id = 79, Name = "Kilis", Districts = new List<string> { "Elbeyli", "Merkez", "Musabeyli", "Polateli" } });
            // Osmaniye
            list.Add(new City { Id = 80, Name = "Osmaniye", Districts = new List<string> { "Bahçe", "Düziçi", "Hasanbeyli", "Kadirli", "Merkez", "Sumbas", "Toprakkale" } });
            // Düzce
            list.Add(new City { Id = 81, Name = "Düzce", Districts = new List<string> { "Akçakoca", "Cumayeri", "Çilimli", "Gölyaka", "Gümüşova", "Kaynaşlı", "Merkez", "Yığılca" } });

            return list.OrderBy(x => x.Name).ToList();
        }
    }
}
