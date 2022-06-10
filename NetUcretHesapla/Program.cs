do
{
	double aylikFaturaTutari, aylikGider, yillikEkGelir, yillikEkGider;
	bool gercekKisiMi;
	string? gercekKisi = string.Empty;

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
		if (gercekKisi == "E" || gercekKisi == "H")
			break;
		else
			Console.WriteLine("Yanlış seçim yaptınız.");
	}
	
		
	gercekKisiMi = gercekKisi == "E" ? true : false;
	
	Dictionary<string, string> hesaplar = NetUcretHesapla(aylikFaturaTutari, aylikGider, yillikEkGelir, yillikEkGider, gercekKisiMi);

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
while (Console.ReadLine() == "E");


static Dictionary<string, string> NetUcretHesapla(double aylikFaturaTutari, double aylikGider, double yillikEkGelir, double yillikEkGider, bool gercekKisiMi)
{
	double gelirVergiOranı = gercekKisiMi ? 0 : 0.25;
	double yillikFaturaTutari = aylikFaturaTutari * 12;

	double aylikKdv = aylikFaturaTutari * 0.18;
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

	double yillikGelirVergisi = gercekKisiMi ? CalcYGV(yillikNetGelir, ref gelirVergiOranı) : (yillikNetGelir) * 0.25;
	double aylikGelirVergisi = yillikGelirVergisi / 12;

	double yillikNetUcret = yillikNetGelir - yillikGelirVergisi;
	double aylikNetUcret = aylikNetGelir - aylikGelirVergisi;

	return new Dictionary<string, string>
	{
		{ "Aylık Fatura Tutarı          ", $"{aylikFaturaTutari :C2}"},
		{ "Yıllık Fatura Tutarı         ", $"{yillikFaturaTutari :C2}"},

		{ "Aylık Katma Değer Vergisi    ", $"{aylikKdv :C2}"},
		{ "Yıllık Katma Değer Vergisi   ", $"{yillikKdv :C2}"},

		{ "Aylık Ciro                   ", $"{aylikCiro :C2}"},
		{ "Yıllık Ciro                  ", $"{yillikCiro :C2}"},

		{ "Aylık Gelir                  ", $"{aylikGelir :C2}"},
		{ "Yıllık Gelir                 ", $"{yillikGelir :C2}"},

		{ "Aylık Gider                  ", $"{aylikGider :C2}"},
		{ "Yıllık Gider                 ", $"{yillikGider :C2}"},

		{ "Aylık Ek Gelir               ", $"{aylikEkGelir :C2}"},
		{ "Yıllık Ek Gelir              ", $"{yillikEkGelir :C2}"},

		{ "Aylık Ek Gider               ", $"{aylikEkGider :C2}"},
		{ "Yıllık Ek Gider              ", $"{yillikEkGider :C2}"},

		{ "Aylık Toplam Gelir           ", $"{aylikToplamGelir :C2}"},
		{ "Yıllık Toplam Gelir          ", $"{yillikToplamGelir :C2}"},

		{ "Aylık Toplam Gider           ", $"{aylikToplamGider :C2}"},
		{ "Yıllık Toplam Gider          ", $"{yillikToplamGider :C2}"},

		{ "Aylık Net Gelir              ", $"{aylikNetGelir :C2}"},
		{ "Yıllık Net Gelir             ", $"{yillikNetGelir :C2}"},

		{ "Aylık Gelir Vergisi          ", $"{aylikGelirVergisi :C2}"},
		{ "Yıllık Gelir Vergisi         ", $"{yillikGelirVergisi :C2}"},

		{ "Aylık Net Ücret              ", $"{aylikNetUcret :C2}"},
		{ "Yıllık Net Ücret             ", $"{yillikNetUcret :C2}"},

		{ "Gelir Verigisi Oranı         ", $"%{gelirVergiOranı*100}"},
	};
}

static double CalcYGV(double yıllikGelir, ref double gelirVergiOranı)
{
	bool ucretGeliriMi = true;
	double vergiMiktari = 0;

	if (yıllikGelir <= 32000)
	{
		gelirVergiOranı = 0.15;
		vergiMiktari = yıllikGelir * gelirVergiOranı;
	}
	else if (yıllikGelir <= 70000)
	{
		gelirVergiOranı = 0.20;
		vergiMiktari = (yıllikGelir - 32000) * gelirVergiOranı + 4800;
	}
	else if (yıllikGelir <= 170000 && !ucretGeliriMi)
	{
		gelirVergiOranı = 0.27;
		vergiMiktari = (yıllikGelir - 70000) * gelirVergiOranı + 12400;
	}
	else if (yıllikGelir <= 250000 && ucretGeliriMi)
	{
		gelirVergiOranı = 0.27;
		vergiMiktari = (yıllikGelir - 70000) * gelirVergiOranı + 12400;
	}
	else if (yıllikGelir <= 250000 && ucretGeliriMi)
	{
		gelirVergiOranı = 0.27;
		vergiMiktari = (yıllikGelir - 70000) * gelirVergiOranı + 12400;
	}
	else if (yıllikGelir <= 880000 && !ucretGeliriMi)
	{
		gelirVergiOranı = 0.35;
		vergiMiktari = (yıllikGelir - 170000) * gelirVergiOranı + 39400;
	}
	else if (yıllikGelir <= 880000 && ucretGeliriMi)
	{
		gelirVergiOranı = 0.35;
		vergiMiktari = (yıllikGelir - 250000) * gelirVergiOranı + 61000;
	}
	else if (yıllikGelir > 880000 && !ucretGeliriMi)
	{
		gelirVergiOranı = 0.40;
		vergiMiktari = (yıllikGelir - 880000) * gelirVergiOranı + 287900;
	}
	else if (yıllikGelir > 880000 && ucretGeliriMi)
	{
		gelirVergiOranı = 0.40;
		vergiMiktari = (yıllikGelir - 880000) * gelirVergiOranı + 281500;
	}

	return vergiMiktari;
}