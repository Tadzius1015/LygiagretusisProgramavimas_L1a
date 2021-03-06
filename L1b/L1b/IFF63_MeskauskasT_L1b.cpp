// Tadas Meškauskas IFF-6/3 L1b darbas
// 1.Kokia tvarka jos yra užrašytos.
// 2.Atsitiktine.
// 3.Atsitiktinį skaičių.
// 4.Atsitiktine.

#include "pch.h"
#include <iostream>
#include <fstream>
#include <string>
#include <omp.h>
#include <iomanip>

std::double_t D[20]; // Masyvas saugoti double reikšmėms(šiuo atveju dviračių kainoms).
std::int32_t I[20];  // Masyvas saugoti int reikšmėms(šiuo atveju dviračių kiekiams).
std::string S[20];   // Masyvas saugoti string reikšmėms(šiuo atveju dviračių gamintojams).
struct Rezultatai    // Struktūra, skirta saugoti duomenims po gijų vykdymo.
{
	std::int16_t IteracijosNr;
	std::string Gamintojas;
	std::int16_t Kiekis;
	std::double_t Kaina;
};
Rezultatai P[20]; // Masyvas, kuris saugo rezultatus po gijų vykdymo(gijos numerius, gamintojus, kiekius bei kainas).
// Skaitymo metodas, kuris skaito pradinius duomenis iš failo.
void Skaitymas(int &n)
{
	std::string eil;
    n = 0; // eilučių kiekis pradiniame faile.
	std::ifstream skait("IFF63_MeskauskasT_L1b_dat.txt");
	if (skait.is_open()) // tikrina ar tas failas yra atidarytas skaitymui(taip pat patikrina ar jis egzistuoja).
	{
		skait >> n;
		for (size_t i = 0; i < n; i++)
		{
			skait >> S[i] >> I[i] >> D[i];
		}
		skait.close(); // skaitomo failo uždarymas po duomenų nuskaitymo.
	}
	else
	{
		std::cout << "Nera tokio failo" << '\n';
	}
}
void Rasymas(int n)
{
	std::ofstream out("IFF63_MeskauskasT_L1b_rez.txt");
	if (out.is_open())
	{
		out << "-----------------------------------------------------------------" << '\n';
		out << "Nuskaicius pradinius duomenis:" << '\n';
		out << "-----------------------------------------------------------------" << '\n';
		out << std::setw(21) << std::left << "Gamintojas" << std::setw(8) << std::right << "Kiekis" << std::setw(12) << std::right << "Kaina" << '\n';
		for (size_t i = 0; i < n; i++)
		{
			out << std::setw(21) << std::left << S[i] << " " << std::setw(7) << std::right << I[i] << " " << std::setw(11) << std::right << D[i] << '\n';
		}
		out << "-----------------------------------------------------------------" << '\n';
		out << "Po giju:" << '\n';
		out << "-----------------------------------------------------------------" << '\n';
		out << std::setw(12) << std::left << "GijosNr" << std::setw(21) << std::left << "Gamintojas" << std::setw(8) << std::right << "Kiekis" << std::setw(12) << std::right << "Kaina" << '\n';
		for (size_t i = 0; i < n; i++)
		{
			out << std::setw(12) << std::left << P[i].IteracijosNr << std::setw(21) << std::left << P[i].Gamintojas << std::setw(8) << std::right << P[i].Kiekis << std::setw(12) << std::right << P[i].Kaina << '\n';
		}
		out << "-----------------------------------------------------------------" << '\n';
		out.close();
	}		
	else
	{
		std::cout << "Rasymo failas neatidarytas." << '\n';
	}
}
int main()
{
	int n = 0;
	Skaitymas(n);
	int gijosNum = 0; 
	int i = 0; // skaitliukas, nurodantis iteraciją.
	omp_set_num_threads(n); // Nustato kiek gijų bus kuriama pradiniams duomenims apdoroti. Šiuo atveju bus sukurta n gijų.
	#pragma omp parallel private(gijosNum)
	{
		int gijosNum = omp_get_thread_num(); // Gražina konkrečios gijos numerį.
		P[i].IteracijosNr = gijosNum;
		P[i].Gamintojas = S[gijosNum];
		P[i].Kiekis = I[gijosNum];
		P[i].Kaina = D[gijosNum];
		i++;
	}	
	Rasymas(n);
}
