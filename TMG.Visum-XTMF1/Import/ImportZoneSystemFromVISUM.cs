using TMG.Visum.Common;

namespace TMG.Visum.Import;

[ModuleInformation(Description = "Load in the zone system from the given VISUM instance.")]
public sealed class ImportZoneSystemFromVISUM : IZoneSystem
{

    [SubModelInformation(Required = true, Description = "The Visum instance to read the zones from.")]
    public IDataSource<VisumInstance> VisumInstance = null!;

    [SubModelInformation(Required = false, Description = "The source to load in planning districts per TAZ.")]
    public IDataSource<SparseArray<float>>? PlanningDistricts;

    [SubModelInformation(Required = false, Description = "The source to load in the regions per TAZ.")]
    public IDataSource<SparseArray<float>>? Regions;

    [SubModelInformation(Required = false, Description = "Load in a population for TAZ.")]
    public IDataSource<SparseArray<float>>? Population;

    [SubModelInformation(Required = false, Description = "Load in a custom matrix to use for distances.")]
    public IDataSource<SparseTwinIndex<float>>? CustomDistances;



    public IZoneSystem? GiveData()
    {
        return this;
    }

    /// <summary>
    /// The zone system.
    /// </summary>
    private SparseArray<IZone>? _zones;

    /// <summary>
    /// The zone that represents a location that is not fixed.
    /// </summary>
    private readonly IZone _roamingZone = new Zone();

    public void LoadData()
    {
        ((Zone)_roamingZone).ZoneNumber = RoamingZoneNumber;
        LoadZones();
        LoadPlanningDistricts();
        LoadRegions();
        LoadPopulation();
        Distances = LoadDistances();
        SetIntrazonals();
    }

    /// <summary>
    /// Load in the TAZ
    /// </summary>
    private void LoadZones()
    {
        var (zoneNumber, x, y) = GetZoneInformation();
        var zones = new IZone[zoneNumber.Length];
        for (var i = 0; i < zones.Length; i++)
        {
            zones[i] = new Zone()
            {
                ZoneNumber = zoneNumber[i],
                X = x[i],
                Y = y[i],
            };
        }
        _zones = SparseArray<IZone>.CreateSparseArray(zoneNumber, zones);
    }

    /// <summary>
    /// Load in the planning districts.
    /// Must run after the zones have been loaded.
    /// </summary>
    private void LoadPlanningDistricts()
    {
        if (PlanningDistricts is null || _zones is null)
        {
            return;
        }

        if (!PlanningDistricts.Loaded)
        {
            PlanningDistricts.LoadData();
        }
        var pds = PlanningDistricts.GiveData()!;
        var flatZones = _zones.GetFlatData();
        var flatPds = pds.GetFlatData();
        for (var i = 0; i < flatPds.Length; i++)
        {
            var sparseIndex = pds.GetSparseIndex(i);
            var index = _zones.GetFlatIndex(sparseIndex);
            if (index >= 0)
            {
                ((Zone)flatZones[index]!).PlanningDistrict = (int)flatPds[i];
            }
        }
    }

    private void SetIntrazonals()
    {
        if (Distances is null || _zones is null)
        {
            return;
        }

        var flatDistance = Distances.GetFlatData();
        var flatZones = _zones.GetFlatData();
        for (var i = 0; i < flatZones.Length; i++)
        {
            var intra = flatDistance[i][i];
            var zone = ((Zone)flatZones[i]!);
            zone.InternalDistance = intra;
            // InternalDistance = (sqrt(Area) * 2) / 6
            // (3 * InternalDistance)^2 = Area
            zone.InternalArea = (intra * intra * 9);
        }
    }

    /// <summary>
    /// Load in the regions
    /// Must run after the zones have been loaded.
    /// </summary>
    private void LoadRegions()
    {
        if (Regions is null || _zones is null)
        {
            return;
        }

        if (!Regions.Loaded)
        {
            Regions.LoadData();
        }
        var regions = Regions.GiveData()!;
        var flatZones = _zones.GetFlatData();
        var flatPds = regions.GetFlatData();
        for (var i = 0; i < flatPds.Length; i++)
        {
            var sparseIndex = regions.GetSparseIndex(i);
            var index = _zones.GetFlatIndex(sparseIndex);
            if (index >= 0)
            {
                ((Zone)flatZones[index]!).RegionNumber = (int)flatPds[i];
            }
        }
    }

    public void LoadPopulation()
    {
        if (Population is null || _zones is null)
        {
            return;
        }

        if (!Population.Loaded)
        {
            Population.LoadData();
        }
        var population = Population.GiveData()!;
        var flatZones = _zones.GetFlatData();
        var flatPop = population.GetFlatData();
        for (var i = 0; i < flatPop.Length; i++)
        {
            var sparseIndex = population.GetSparseIndex(i);
            var index = _zones.GetFlatIndex(sparseIndex);
            if (index >= 0)
            {
                ((Zone)flatZones[index]!).Population = (int)flatPop[i];
            }
        }
    }

    /// <summary>
    /// Load the distances
    /// Must run after the zones have been loaded.
    /// </summary>
    private SparseTwinIndex<float> LoadDistances()
    {
        if (CustomDistances is not null)
        {
            if (!CustomDistances.Loaded)
            {
                CustomDistances.LoadData();
            }
            return CustomDistances.GiveData()!;
        }
        else
        {
            // If there are no distances compute straight line distances between zones
            var flatZones = _zones!.GetFlatData();
            var distances = _zones.CreateSquareTwinArray<float>();
            var flatDistances = distances.GetFlatData();
            var x = flatZones.Select(z => z!.X).ToArray();
            var y = flatZones.Select(z => z!.Y).ToArray();
            for (var i = 0; i < flatDistances.Length; i++)
            {
                for (var j = 0; j < flatDistances[i].Length; j++)
                {
                    var deltaX = x[i] - x[j];
                    var deltaY = y[i] - y[j];
                    flatDistances[i][j] = MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);
                }
            }
            return distances;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private (int[] zoneNumber, float[] x, float[] y) GetZoneInformation()
    {
        if (!VisumInstance.Loaded)
        {
            VisumInstance.LoadData();
        }
        var visum = VisumInstance.GiveData();
        return visum!.GetZoneInformation();
    }

    [RunParameter("Keep Loaded", true, "Don't unload the zone system.")]
    public bool KeepLoaded;

    public void UnloadData()
    {
        if (!KeepLoaded)
        {
            // Release the zones
            _zones = null;
            Distances = null;
            PlanningDistricts?.UnloadData();
            Regions?.UnloadData();
        }
    }

    public bool Loaded => _zones is not null;

    public bool RuntimeValidation(ref string? error)
    {
        return true;
    }

    public string Name { get; set; } = null!;

    public float Progress => 0f;

    public Tuple<byte, byte, byte> ProgressColour => new(50, 150, 50);

    public IZone? Get(int zoneNumber)
    {
        if (_zones?.TryRead(zoneNumber, out IZone? zone) == true)
        {
            return zone;
        }
        if (zoneNumber == RoamingZoneNumber)
        {
            return _roamingZone;
        }
        return null;
    }

    public SparseTwinIndex<float>? Distances { get; private set; }

    public int NumberOfExternalZones => 0;

    public int NumberOfInternalZones => NumberOfZones;

    public int NumberOfZones => _zones?.Count ?? 0;

    [RunParameter("Roaming Zone Number", 8888, "The zone number that represents a place that does not have a fixed location.")]
    public int RoamingZoneNumber { get; set; }

    public SparseArray<IZone>? ZoneArray => _zones;

}
