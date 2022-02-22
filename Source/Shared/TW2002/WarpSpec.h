#pragma once

#include <string>

class Sectors
{
private:
    long** sectors;

public:
    Sectors(int max) {
        sectors = new long* [max + 1];
        for (int count = 0; count <= max; count++)
            sectors[count] = new long[6];
    }

    operator long** () {
        return sectors;
    }

    //void operator=(const long** val) {
    //    value[][] = val;
    //}
};

class WarpSpec {
public:
    //Sectors Prime;
    //Sectors Mirror;

    WarpSpec(long maxSectors);
    void BigBang();
private:



};




extern "C" __declspec(dllexport) void* Create(long maxSectors) {
    return (void*) new WarpSpec(maxSectors);
}
extern "C" __declspec(dllexport) void* Load(WarpSpec * ws, int x) {
    return (void*) ws->BigBang();
}

