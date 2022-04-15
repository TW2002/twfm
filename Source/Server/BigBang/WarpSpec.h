#pragma once

#include <iostream>
#include <vector>
#include <random>
#include <filesystem>

//using namespace std;
using std::cout;
using std::vector;
using std::mt19937;
using std::string;
using namespace std::filesystem;
using namespace std::chrono;


namespace TradeWars {
    class WarpSpec
    {
#pragma region WarpSpec
    public:
        long MaxSectors;
        long MaxCourse;

        long*** Sectors;
        bool** Searched;

        vector<long> Avoids;

        /// <summary>
        /// Warpspec Constructor - Creates an empty warpspec.
        /// </summary>
        /// <param name="maxSectors">Maximum number of sectors (Default = 0).</param>
        WarpSpec(long maxSectors = 0);

        // Destructor.
        ~WarpSpec();

        /// <summary>
        /// Load a warpspec from a file (Text Format).
        /// </summary>
        /// <param name="folder">Full path to parrent folder.</param>
        /// <param name="filename">Name of file to load.</param>
        void Load(path folder, path filename);

        /// <summary>
        /// Load a warpspec from a file (Text Format).
        /// </summary>
        /// <param name="folder">Full path to parrent folder.</param>
        /// <param name="filename">Name of file to load.</param>
        void Save(path folder, path filename);

        void ResetSearched();
    private:
        void ClearWarps();
        void Dispose();

        long* GetWarps(string& str);
#pragma endregion
#pragma region BigBang
    public:
        mt19937 mt;
        void BigBang(int maxCoarse, int oneWay, int twoWay);

        long NextSector(long  max = 0);
        int NextInt(int max);
        int NextInt(int min, int max);
    private:
        int CourseLength;
        long* Distance;
        long ClassZero[4];


        void WriteTime(high_resolution_clock::time_point start, high_resolution_clock::time_point finish);
        void ClearMap();
        void AddWarp(int origin = 0, int dest = 0);
        void AddWarp(bool Single, int dest = 0);
        bool AddWarp(int origin, int dest, bool single);
        void LoadBaseMap();
        void CreateWarps(int oneWay, int twoWay);
        void LinkUnreachable();
        void MarkCluster(int sector);
        void LimitCoarses();
        void MirrorMap();

        vector<long> GetRoute(int origin = 1, int target = 0);
        vector<long> GetRoute(int origin, int target, int max);
        vector<long> GetRoute(vector<long > map, long  target, int max, int dist);

#pragma endregion
#pragma region QuickRoute
    public:
        vector<vector<long>> AllRoutes;

        void QuickRoute(long origin, long target, int maxCourse = 0);

        void RouteTest(int test);
        void DisplayRoutes();
    private:
        //vector<long> FindRoutes(long* origin, long* target, short front, short back, short maxCourse, short distance = 0);
        //long FindSector(long* origin, short front, long target, int universe = 0);

        vector<long> FindRoutes(vector<long>& origin, vector<long>& target, int maxCourse, int distance = 0);
        long FindSector(vector<long> origin, long target, int universe = 0);
        //void NextSectors(vector<long>& nextSectors, vector<long>& origin, vector<long>& target, int universe = 0);
        //vector<long> FindSectors(vector<long>& origin, vector<long>& target, int universe = 0);

#pragma endregion
    };
}