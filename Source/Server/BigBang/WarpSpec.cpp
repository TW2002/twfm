#include "TradeWars.h"
#include "WarpSpec.h"

namespace TradeWars {
    WarpSpec::WarpSpec(long maxSectors) {
        MaxSectors = maxSectors;
        MaxCourse = 45;

        Sectors = nullptr;
        Searched = nullptr;

        if (MaxSectors > 0) ClearWarps();


        cout << "Empty warpspec created." << endl << endl;

        // Using a hash of random device to generate a unique seed
        random_device rd;
        hash<unsigned> hash_uint;
        auto seed = hash_uint(rd());

        // Initialized the mersenne twister random number generator.
        mt = mt19937(seed);
        cout << "Using random seed: " << seed << endl;
    }

    WarpSpec::~WarpSpec() {
        Dispose();
    }

    void WarpSpec::ClearWarps() {
        if (Sectors != nullptr) Dispose();

        Sectors = new long** [MaxSectors + 1];
        for (int sector = 0; sector <= MaxSectors; sector++) {
            Sectors[sector] = new long* [10];
            for (int warp = 0; warp < 10; warp++) {
                Sectors[sector][warp] = new long[2]{ 0 };
            }
        }

        Distance = new long[MaxSectors + 1]{ 0 };
        for (int count = 1; count < 4; count++) ClassZero[count] = 1;
        ClassZero[0] = 1;

    }

    void WarpSpec::ResetSearched() {
        if (Searched != nullptr) {
            for (int sector = 0; sector <= MaxSectors; sector++) {
                delete[] Searched[sector];
            }
            delete[] Searched;
        }

        Searched = new bool* [MaxSectors + 1];
        for (int sector = 0; sector <= MaxSectors; sector++)
            Searched[sector] = new bool[2]{ false };

        //for (long sector = 0; sector <= MaxSectors; sector++) {
        //    Searched[sector][Prime] = false;
        //    Searched[sector][Mirror] = false;
        //}

        for (long avoid : Avoids) {
            Searched[avoid][Prime] = true;
            Searched[avoid][Mirror] = true;
        }

    }

    void WarpSpec::Dispose() {
        if (Sectors != nullptr) {
            // Delete sectors array
            for (int sector = 0; sector <= MaxSectors; sector++) {
                for (int warp = 0; warp < 10; warp++) {
                    delete[] Sectors[sector][warp];
                }
                delete[] Sectors[sector];
            }
            delete[] Sectors;
        }

        if (Searched != nullptr) {
            // Deleted searched array.
            for (int sector = 0; sector < MaxSectors; sector++)
                delete Searched[sector];
            delete[] Searched;
        }
    }
    void WarpSpec::Load(path folder, path file) {
        path filename(folder);
        filename /= file;

        fstream warpfile;
        warpfile.open(filename, ios::in);
        if (!warpfile.is_open()) {
            cout << "Unble to open """ << file << """ to import warpspec." << endl << endl;
            return;
        }
        else {
            string buffer;
            long sectors = 0;
            long lineCount = 0;
            long warpCount = 0;

            wcout << "Reading warpspec file..." << endl;

            // Get the sector count from the first word.
            while (getline(warpfile, buffer)) {
                string word = buffer.substr(0, buffer.find(" "));

                long* warps = GetWarps(buffer);
                long sector = warps[0];

                if (sector > sectors) {
                    sectors = sector;
                    lineCount++;
                }
            }

            warpfile.clear();
            warpfile.seekg(0);

            cout << "Processing " << lineCount << " sectors." << endl;


            MaxSectors = sectors;
            ClearWarps();

            while (getline(warpfile, buffer)) {
                long* warps = GetWarps(buffer);
                long sector = warps[0];

                for (int warpout = 0; warpout < 6 && warps[warpout + 1] > 0; warpout++) {
                    Sectors[sector][warpout][Prime] = warps[warpout + 1];

                    for (int warpin = 0; warpin < 10; warpin++) {
                        if (Sectors[warps[warpout + 1]][warpin][Mirror] == 0) {
                            Sectors[warps[warpout + 1]][warpin][Mirror] = sector;
                            warpCount++;
                            break;
                        }
                    }

                }

            }
            warpfile.close();
            wcout << "Loaded " << warpCount << " warps." << endl << endl;

        }
    }

    long* WarpSpec::GetWarps(string& str)
    {
        static long warps[7];
        size_t start;
        size_t end = 0;
        int count = 0;

        for (int warp = 0; warp < 7; warp++) warps[warp] = 0;

        while ((start = str.find_first_not_of(" ", end)) != string::npos)
        {
            end = str.find(" ", start);
            try
            {
                warps[count] = stol(str.substr(start, end - start));
                count++;
            }
            catch (const std::exception&) {}  // Ignore exception on failed parse.
        }

        return warps;
    }

    void WarpSpec::Save(path folder, path file) {
        path filename(folder);
        filename /= file;

        if (Sectors == nullptr) {
            cout << "Please load or create a database before saving." << endl << endl;
            return;
        }

        wcout << "Writing warpspec file..." << endl;

        fstream warpfile;
        warpfile.open(filename, ios::out);
        if (warpfile.is_open()) {
            warpfile << ":";

            for (long sector = 1; sector <= MaxSectors; sector++) {
                warpfile << "\n" << sector;

                for (int warp = 0; warp < 10; warp++)
                    if (Sectors[sector][warp][Mirror] > 0)
                        warpfile << setfill(' ') << setw(6) << Sectors[sector][warp][Mirror];
            }

            warpfile << "\n:\n\n";
        }

        cout << "Saved " << MaxSectors << " sectors." << endl << endl;
        warpfile.close();

    }

}
