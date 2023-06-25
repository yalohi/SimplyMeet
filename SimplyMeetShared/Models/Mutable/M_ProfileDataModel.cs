namespace SimplyMeetShared.Models;

public record M_ProfileDataModel
{
	public IEnumerable<PronounsModel> AllPronouns { get; set; } = default;
	public IEnumerable<SexModel> AllSexes { get; set; } = default;
	public IEnumerable<GenderModel> AllGenders { get; set; } = default;
	public IEnumerable<RegionModel> AllRegions { get; set; } = default;
	public IEnumerable<CountryModel> AllCountries { get; set; } = default;
	public IEnumerable<SexualityModel> AllSexualities { get; set; } = default;
};