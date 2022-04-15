// BigBang.cpp : TradeWars universe creator
#include "TradeWars.h"
#include "WarpSpec.h"

using namespace TradeWars;

int main(int argc, char* argv[])
{
	path appPath(path(argv[0]).parent_path());

	//const int MaxSectors = 5 * 1000 * 1000;  // 5M
	const int MaxSectors = 100000 - 1;
	const int TwoWay = (int)(MaxSectors * .80);
	const int OneWay = (int)(MaxSectors * .30);
	const int MaxCoarse = 45;
	WarpSpec ws100k(MaxSectors);
	ws100k.BigBang(MaxCoarse, OneWay, TwoWay);
	ws100k.Save(appPath, "100K-mirror.txt");
}
