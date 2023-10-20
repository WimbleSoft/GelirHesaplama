do
{
	double aylikFaturaTutari, aylikGider, yillikEkGelir, yillikEkGider;
	bool gercekKisiMi = false;
	bool gencGirisimciMi = false;
	string? gercekKisi = string.Empty;
	string? gencGirisimci = string.Empty;

	Console.Clear();
	Console.InputEncoding = System.Text.Encoding.UTF8;
	Console.OutputEncoding = System.Text.Encoding.UTF8;
	Console.Write("Aylık fatura tutarını yazınız:");
	aylikFaturaTutari = Convert.ToDouble(Console.ReadLine());
	Console.Write("Aylık gider tutarını yazınız:");
	aylikGider = Convert.ToDouble(Console.ReadLine());
	Console.Write("Yıllık ek gelir tutarını yazınız:");
	yillikEkGelir = Convert.ToDouble(Console.ReadLine());
	Console.Write("Yıllık ek gider tutarını yazınız:");
    yillikEkGider = Convert.ToDouble(Console.ReadLine());
    while (true)
	{
		Console.Write("Gerçek Kişi Firması mı? (E/H):");
		gercekKisi = Console.ReadLine();
		if (gercekKisi == "E" || gercekKisi == "H" || gercekKisi == "e" || gercekKisi == "h")
			break;
		else
			Console.WriteLine("Yanlış seçim yaptınız.");
	}
	gercekKisiMi = gercekKisi == "E" || gercekKisi == "e";
	while (gercekKisiMi)
	{
		Console.Write("Genç Girişimci mi? (E/H):");
		gencGirisimci = Console.ReadLine();
		if (gercekKisi == "E" || gercekKisi == "H" || gercekKisi == "e" || gercekKisi == "h")
			break;
		else
			Console.WriteLine("Yanlış seçim yaptınız.");
	}
	gencGirisimciMi = gencGirisimci == "E" || gercekKisi == "e";
	
	Dictionary<string, string> hesaplar = NetUcretHesapla(aylikFaturaTutari, aylikGider, yillikEkGelir, yillikEkGider, gercekKisiMi, gencGirisimciMi);

	int i = 0;
	hesaplar.ToList().ForEach(x =>
	{
		if (i % 2 == 0)
			Console.WriteLine();
		Console.WriteLine($"{x.Key}: {x.Value}");
		i++;
	});

	Console.WriteLine();
	Console.WriteLine("Tekrar girmek ister misiniz? (E/H):");
}
while (Console.ReadLine() == "E" || Console.ReadLine() == "e");


static Dictionary<string, string> NetUcretHesapla(double aylikFaturaTutari, double aylikGider, double yillikEkGelir, double yillikEkGider, bool gercekKisiMi, bool gencGirisimciMi)
{
	double istisnaiGelir = gencGirisimciMi ? 150000 : 0;
	double gelirVergiOranı = gercekKisiMi ? 0 : 0.25;
	double yillikFaturaTutari = aylikFaturaTutari * 12;

	double aylikKdv = aylikFaturaTutari * 0.20;
	double yillikKdv = aylikKdv * 12;

	double aylikCiro = aylikFaturaTutari + aylikKdv;
	double yillikCiro = aylikCiro * 12;

	double yillikGider = aylikGider * 12;

	double aylikGelir = aylikCiro - aylikKdv;
	double yillikGelir = yillikCiro - yillikKdv;

	double aylikEkGelir = yillikEkGelir / 12;
	double aylikEkGider = yillikEkGider / 12;

	double aylikToplamGelir = aylikGelir + aylikEkGelir;
	double yillikToplamGelir = yillikGelir + yillikEkGelir;

	double aylikToplamGider = aylikGider + aylikEkGider;
	double yillikToplamGider = yillikGider + yillikEkGider;

	double aylikNetGelir = aylikToplamGelir - aylikToplamGider;
	double yillikNetGelir = yillikToplamGelir - yillikToplamGider;

	double yillikGelirVergisi = gercekKisiMi ? CalcYGV(yillikNetGelir - istisnaiGelir, ref gelirVergiOranı) : (yillikNetGelir) * 0.25;
	double aylikGelirVergisi = yillikGelirVergisi / 12;

	double yillikNetUcret = yillikNetGelir - yillikGelirVergisi;
	double aylikNetUcret = aylikNetGelir - aylikGelirVergisi;

	return new Dictionary<string, string>
	{
		{ "Aylık Ortalama Fatura Tutarı ", $"{aylikFaturaTutari :C2}"},
		{ "Yıllık Fatura Tutarı         ", $"{yillikFaturaTutari :C2}"},

		{ "Aylık Katma Değer Vergisi    ", $"{aylikKdv :C2}"},
		{ "Yıllık Katma Değer Vergisi   ", $"{yillikKdv :C2}"},

		{ "Aylık Ortalama Ciro          ", $"{aylikCiro :C2}"},
		{ "Yıllık Ciro                  ", $"{yillikCiro :C2}"},

		{ "Aylık Ortalama Gelir         ", $"{aylikGelir :C2}"},
		{ "Yıllık Gelir                 ", $"{yillikGelir :C2}"},

		{ "Aylık Ortalama Gider         ", $"{aylikGider :C2}"},
		{ "Yıllık Gider                 ", $"{yillikGider :C2}"},

		{ "Aylık Ortalama Ek Gelir      ", $"{aylikEkGelir :C2}"},
		{ "Yıllık Ek Gelir              ", $"{yillikEkGelir :C2}"},

		{ "Aylık Ortalama Ek Gider      ", $"{aylikEkGider :C2}"},
		{ "Yıllık Ek Gider              ", $"{yillikEkGider :C2}"},

		{ "Aylık Ortalama Toplam Gelir  ", $"{aylikToplamGelir :C2}"},
		{ "Yıllık Toplam Gelir          ", $"{yillikToplamGelir :C2}"},

		{ "Aylık Ortalama Toplam Gider  ", $"{aylikToplamGider :C2}"},
		{ "Yıllık Toplam Gider          ", $"{yillikToplamGider :C2}"},

		{ "Aylık Ortalama Net Gelir     ", $"{aylikNetGelir :C2}"},
		{ "Yıllık Net Gelir             ", $"{yillikNetGelir :C2}"},

		{ "Gelir Verigisi Oranı         ", $"%{gelirVergiOranı*100}"},
		{ "İstisnai Gelir               ", $"{istisnaiGelir :C2}"},

		{ "Aylık Ortalama Gelir Vergisi ", $"{aylikGelirVergisi :C2}"},
		{ "Yıllık Gelir Vergisi         ", $"{yillikGelirVergisi :C2}"},

		{ "Aylık Ortalama Net Ücret     ", $"{aylikNetUcret :C2}"},
		{ "Yıllık Net Ücret             ", $"{yillikNetUcret :C2}"},

	};
}

static double CalcYGV(double yıllikGelir, ref double gelirVergiOranı)
{
	bool ucretGeliriMi = true;
	double vergiMiktari = 0;

	if (yıllikGelir <= 70000)
	{
		gelirVergiOranı = 0.15;
		vergiMiktari = yıllikGelir * gelirVergiOranı;
	}
	else if (yıllikGelir <= 150000)
	{
		gelirVergiOranı = 0.20;
		vergiMiktari = (yıllikGelir - 70000) * gelirVergiOranı + 10500;
	}

	else if (yıllikGelir <= 370000 && !ucretGeliriMi)
	{
		gelirVergiOranı = 0.27;
		vergiMiktari = (yıllikGelir - 150000) * gelirVergiOranı + 26500;
	}
	else if (yıllikGelir <= 550000 && ucretGeliriMi)
	{
		gelirVergiOranı = 0.27;
		vergiMiktari = (yıllikGelir - 150000) * gelirVergiOranı + 26500;
    }

    else if (yıllikGelir <= 1900000 && !ucretGeliriMi)
    {
        gelirVergiOranı = 0.35;
        vergiMiktari = (yıllikGelir - 550000) * gelirVergiOranı + 85900;
    }
    else if (yıllikGelir <= 1900000 && ucretGeliriMi)
	{
		gelirVergiOranı = 0.35;
		vergiMiktari = (yıllikGelir - 550000) * gelirVergiOranı + 134500;
	}

	else if (yıllikGelir > 1900000 && !ucretGeliriMi)
	{
		gelirVergiOranı = 0.40;
		vergiMiktari = (yıllikGelir - 1900000) * gelirVergiOranı + 621400;
	}
	else if (yıllikGelir > 1900000 && ucretGeliriMi)
	{
		gelirVergiOranı = 0.40;
		vergiMiktari = (yıllikGelir - 1900000) * gelirVergiOranı + 607000;
	}
	else
	{
		gelirVergiOranı = 0;
        vergiMiktari = -1;
	}

	return vergiMiktari;
}