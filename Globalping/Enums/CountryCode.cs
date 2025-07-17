using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Globalping;

/// <summary>
/// ISO 3166-1 alpha-2 country codes.
/// </summary>
[JsonConverter(typeof(CountryCodeConverter))]
public enum CountryCode
{
    [EnumMember(Value = "AF")] Afghanistan,
    [EnumMember(Value = "AX")] AlandIslands,
    [EnumMember(Value = "AL")] Albania,
    [EnumMember(Value = "DZ")] Algeria,
    [EnumMember(Value = "AS")] AmericanSamoa,
    [EnumMember(Value = "AD")] Andorra,
    [EnumMember(Value = "AO")] Angola,
    [EnumMember(Value = "AI")] Anguilla,
    [EnumMember(Value = "AQ")] Antarctica,
    [EnumMember(Value = "AG")] AntiguaAndBarbuda,
    [EnumMember(Value = "AR")] Argentina,
    [EnumMember(Value = "AM")] Armenia,
    [EnumMember(Value = "AW")] Aruba,
    [EnumMember(Value = "AU")] Australia,
    [EnumMember(Value = "AT")] Austria,
    [EnumMember(Value = "AZ")] Azerbaijan,
    [EnumMember(Value = "BS")] Bahamas,
    [EnumMember(Value = "BH")] Bahrain,
    [EnumMember(Value = "BD")] Bangladesh,
    [EnumMember(Value = "BB")] Barbados,
    [EnumMember(Value = "BY")] Belarus,
    [EnumMember(Value = "BE")] Belgium,
    [EnumMember(Value = "BZ")] Belize,
    [EnumMember(Value = "BJ")] Benin,
    [EnumMember(Value = "BM")] Bermuda,
    [EnumMember(Value = "BT")] Bhutan,
    [EnumMember(Value = "BO")] BoliviaPlurinationalStateOf,
    [EnumMember(Value = "BQ")] BonaireSintEustatiusAndSaba,
    [EnumMember(Value = "BA")] BosniaAndHerzegovina,
    [EnumMember(Value = "BW")] Botswana,
    [EnumMember(Value = "BV")] BouvetIsland,
    [EnumMember(Value = "BR")] Brazil,
    [EnumMember(Value = "IO")] BritishIndianOceanTerritory,
    [EnumMember(Value = "BN")] BruneiDarussalam,
    [EnumMember(Value = "BG")] Bulgaria,
    [EnumMember(Value = "BF")] BurkinaFaso,
    [EnumMember(Value = "BI")] Burundi,
    [EnumMember(Value = "KH")] Cambodia,
    [EnumMember(Value = "CM")] Cameroon,
    [EnumMember(Value = "CA")] Canada,
    [EnumMember(Value = "CV")] CapeVerde,
    [EnumMember(Value = "KY")] CaymanIslands,
    [EnumMember(Value = "CF")] CentralAfricanRepublic,
    [EnumMember(Value = "TD")] Chad,
    [EnumMember(Value = "CL")] Chile,
    [EnumMember(Value = "CN")] China,
    [EnumMember(Value = "CX")] ChristmasIsland,
    [EnumMember(Value = "CC")] CocosKeelingIslands,
    [EnumMember(Value = "CO")] Colombia,
    [EnumMember(Value = "KM")] Comoros,
    [EnumMember(Value = "CG")] Congo,
    [EnumMember(Value = "CD")] CongoTheDemocraticRepublicOfThe,
    [EnumMember(Value = "CK")] CookIslands,
    [EnumMember(Value = "CR")] CostaRica,
    [EnumMember(Value = "CI")] CoteDIvoire,
    [EnumMember(Value = "HR")] Croatia,
    [EnumMember(Value = "CU")] Cuba,
    [EnumMember(Value = "CW")] Curacao,
    [EnumMember(Value = "CY")] Cyprus,
    [EnumMember(Value = "CZ")] CzechRepublic,
    [EnumMember(Value = "DK")] Denmark,
    [EnumMember(Value = "DJ")] Djibouti,
    [EnumMember(Value = "DM")] Dominica,
    [EnumMember(Value = "DO")] DominicanRepublic,
    [EnumMember(Value = "EC")] Ecuador,
    [EnumMember(Value = "EG")] Egypt,
    [EnumMember(Value = "SV")] ElSalvador,
    [EnumMember(Value = "GQ")] EquatorialGuinea,
    [EnumMember(Value = "ER")] Eritrea,
    [EnumMember(Value = "EE")] Estonia,
    [EnumMember(Value = "ET")] Ethiopia,
    [EnumMember(Value = "FK")] FalklandIslandsMalvinas,
    [EnumMember(Value = "FO")] FaroeIslands,
    [EnumMember(Value = "FJ")] Fiji,
    [EnumMember(Value = "FI")] Finland,
    [EnumMember(Value = "FR")] France,
    [EnumMember(Value = "GF")] FrenchGuiana,
    [EnumMember(Value = "PF")] FrenchPolynesia,
    [EnumMember(Value = "TF")] FrenchSouthernTerritories,
    [EnumMember(Value = "GA")] Gabon,
    [EnumMember(Value = "GM")] Gambia,
    [EnumMember(Value = "GE")] Georgia,
    [EnumMember(Value = "DE")] Germany,
    [EnumMember(Value = "GH")] Ghana,
    [EnumMember(Value = "GI")] Gibraltar,
    [EnumMember(Value = "GR")] Greece,
    [EnumMember(Value = "GL")] Greenland,
    [EnumMember(Value = "GD")] Grenada,
    [EnumMember(Value = "GP")] Guadeloupe,
    [EnumMember(Value = "GU")] Guam,
    [EnumMember(Value = "GT")] Guatemala,
    [EnumMember(Value = "GG")] Guernsey,
    [EnumMember(Value = "GN")] Guinea,
    [EnumMember(Value = "GW")] GuineaBissau,
    [EnumMember(Value = "GY")] Guyana,
    [EnumMember(Value = "HT")] Haiti,
    [EnumMember(Value = "HM")] HeardIslandAndMcdonaldIslands,
    [EnumMember(Value = "VA")] HolySeeVaticanCityState,
    [EnumMember(Value = "HN")] Honduras,
    [EnumMember(Value = "HK")] HongKong,
    [EnumMember(Value = "HU")] Hungary,
    [EnumMember(Value = "IS")] Iceland,
    [EnumMember(Value = "IN")] India,
    [EnumMember(Value = "ID")] Indonesia,
    [EnumMember(Value = "IR")] IranIslamicRepublicOf,
    [EnumMember(Value = "IQ")] Iraq,
    [EnumMember(Value = "IE")] Ireland,
    [EnumMember(Value = "IM")] IsleOfMan,
    [EnumMember(Value = "IL")] Israel,
    [EnumMember(Value = "IT")] Italy,
    [EnumMember(Value = "JM")] Jamaica,
    [EnumMember(Value = "JP")] Japan,
    [EnumMember(Value = "JE")] Jersey,
    [EnumMember(Value = "JO")] Jordan,
    [EnumMember(Value = "KZ")] Kazakhstan,
    [EnumMember(Value = "KE")] Kenya,
    [EnumMember(Value = "KI")] Kiribati,
    [EnumMember(Value = "KP")] KoreaDemocraticPeopleSRepublicOf,
    [EnumMember(Value = "KR")] KoreaRepublicOf,
    [EnumMember(Value = "KW")] Kuwait,
    [EnumMember(Value = "KG")] Kyrgyzstan,
    [EnumMember(Value = "LA")] LaoPeopleSDemocraticRepublic,
    [EnumMember(Value = "LV")] Latvia,
    [EnumMember(Value = "LB")] Lebanon,
    [EnumMember(Value = "LS")] Lesotho,
    [EnumMember(Value = "LR")] Liberia,
    [EnumMember(Value = "LY")] Libya,
    [EnumMember(Value = "LI")] Liechtenstein,
    [EnumMember(Value = "LT")] Lithuania,
    [EnumMember(Value = "LU")] Luxembourg,
    [EnumMember(Value = "MO")] Macao,
    [EnumMember(Value = "MK")] MacedoniaTheFormerYugoslavRepublicOf,
    [EnumMember(Value = "MG")] Madagascar,
    [EnumMember(Value = "MW")] Malawi,
    [EnumMember(Value = "MY")] Malaysia,
    [EnumMember(Value = "MV")] Maldives,
    [EnumMember(Value = "ML")] Mali,
    [EnumMember(Value = "MT")] Malta,
    [EnumMember(Value = "MH")] MarshallIslands,
    [EnumMember(Value = "MQ")] Martinique,
    [EnumMember(Value = "MR")] Mauritania,
    [EnumMember(Value = "MU")] Mauritius,
    [EnumMember(Value = "YT")] Mayotte,
    [EnumMember(Value = "MX")] Mexico,
    [EnumMember(Value = "FM")] MicronesiaFederatedStatesOf,
    [EnumMember(Value = "MD")] MoldovaRepublicOf,
    [EnumMember(Value = "MC")] Monaco,
    [EnumMember(Value = "MN")] Mongolia,
    [EnumMember(Value = "ME")] Montenegro,
    [EnumMember(Value = "MS")] Montserrat,
    [EnumMember(Value = "MA")] Morocco,
    [EnumMember(Value = "MZ")] Mozambique,
    [EnumMember(Value = "MM")] Myanmar,
    [EnumMember(Value = "NA")] Namibia,
    [EnumMember(Value = "NR")] Nauru,
    [EnumMember(Value = "NP")] Nepal,
    [EnumMember(Value = "NL")] Netherlands,
    [EnumMember(Value = "NC")] NewCaledonia,
    [EnumMember(Value = "NZ")] NewZealand,
    [EnumMember(Value = "NI")] Nicaragua,
    [EnumMember(Value = "NE")] Niger,
    [EnumMember(Value = "NG")] Nigeria,
    [EnumMember(Value = "NU")] Niue,
    [EnumMember(Value = "NF")] NorfolkIsland,
    [EnumMember(Value = "MP")] NorthernMarianaIslands,
    [EnumMember(Value = "NO")] Norway,
    [EnumMember(Value = "OM")] Oman,
    [EnumMember(Value = "PK")] Pakistan,
    [EnumMember(Value = "PW")] Palau,
    [EnumMember(Value = "PS")] PalestineStateOf,
    [EnumMember(Value = "PA")] Panama,
    [EnumMember(Value = "PG")] PapuaNewGuinea,
    [EnumMember(Value = "PY")] Paraguay,
    [EnumMember(Value = "PE")] Peru,
    [EnumMember(Value = "PH")] Philippines,
    [EnumMember(Value = "PN")] Pitcairn,
    [EnumMember(Value = "PL")] Poland,
    [EnumMember(Value = "PT")] Portugal,
    [EnumMember(Value = "PR")] PuertoRico,
    [EnumMember(Value = "QA")] Qatar,
    [EnumMember(Value = "RE")] Reunion,
    [EnumMember(Value = "RO")] Romania,
    [EnumMember(Value = "RU")] RussianFederation,
    [EnumMember(Value = "RW")] Rwanda,
    [EnumMember(Value = "BL")] SaintBarthelemy,
    [EnumMember(Value = "SH")] SaintHelenaAscensionAndTristanDaCunha,
    [EnumMember(Value = "KN")] SaintKittsAndNevis,
    [EnumMember(Value = "LC")] SaintLucia,
    [EnumMember(Value = "MF")] SaintMartinFrenchPart,
    [EnumMember(Value = "PM")] SaintPierreAndMiquelon,
    [EnumMember(Value = "VC")] SaintVincentAndTheGrenadines,
    [EnumMember(Value = "WS")] Samoa,
    [EnumMember(Value = "SM")] SanMarino,
    [EnumMember(Value = "ST")] SaoTomeAndPrincipe,
    [EnumMember(Value = "SA")] SaudiArabia,
    [EnumMember(Value = "SN")] Senegal,
    [EnumMember(Value = "RS")] Serbia,
    [EnumMember(Value = "SC")] Seychelles,
    [EnumMember(Value = "SL")] SierraLeone,
    [EnumMember(Value = "SG")] Singapore,
    [EnumMember(Value = "SX")] SintMaartenDutchPart,
    [EnumMember(Value = "SK")] Slovakia,
    [EnumMember(Value = "SI")] Slovenia,
    [EnumMember(Value = "SB")] SolomonIslands,
    [EnumMember(Value = "SO")] Somalia,
    [EnumMember(Value = "ZA")] SouthAfrica,
    [EnumMember(Value = "GS")] SouthGeorgiaAndTheSouthSandwichIslands,
    [EnumMember(Value = "SS")] SouthSudan,
    [EnumMember(Value = "ES")] Spain,
    [EnumMember(Value = "LK")] SriLanka,
    [EnumMember(Value = "SD")] Sudan,
    [EnumMember(Value = "SR")] Suriname,
    [EnumMember(Value = "SJ")] SvalbardAndJanMayen,
    [EnumMember(Value = "SZ")] Eswatini,
    [EnumMember(Value = "SE")] Sweden,
    [EnumMember(Value = "CH")] Switzerland,
    [EnumMember(Value = "SY")] SyrianArabRepublic,
    [EnumMember(Value = "TW")] TaiwanProvinceOfChina,
    [EnumMember(Value = "TJ")] Tajikistan,
    [EnumMember(Value = "TZ")] TanzaniaUnitedRepublicOf,
    [EnumMember(Value = "TH")] Thailand,
    [EnumMember(Value = "TL")] TimorLeste,
    [EnumMember(Value = "TG")] Togo,
    [EnumMember(Value = "TK")] Tokelau,
    [EnumMember(Value = "TO")] Tonga,
    [EnumMember(Value = "TT")] TrinidadAndTobago,
    [EnumMember(Value = "TN")] Tunisia,
    [EnumMember(Value = "TR")] Turkey,
    [EnumMember(Value = "TM")] Turkmenistan,
    [EnumMember(Value = "TC")] TurksAndCaicosIslands,
    [EnumMember(Value = "TV")] Tuvalu,
    [EnumMember(Value = "UG")] Uganda,
    [EnumMember(Value = "UA")] Ukraine,
    [EnumMember(Value = "AE")] UnitedArabEmirates,
    [EnumMember(Value = "GB")] UnitedKingdom,
    [EnumMember(Value = "US")] UnitedStates,
    [EnumMember(Value = "UM")] UnitedStatesMinorOutlyingIslands,
    [EnumMember(Value = "UY")] Uruguay,
    [EnumMember(Value = "UZ")] Uzbekistan,
    [EnumMember(Value = "VU")] Vanuatu,
    [EnumMember(Value = "VE")] VenezuelaBolivarianRepublicOf,
    [EnumMember(Value = "VN")] VietNam,
    [EnumMember(Value = "VG")] VirginIslandsBritish,
    [EnumMember(Value = "VI")] VirginIslandsUS,
    [EnumMember(Value = "WF")] WallisAndFutuna,
    [EnumMember(Value = "EH")] WesternSahara,
    [EnumMember(Value = "YE")] Yemen,
    [EnumMember(Value = "ZM")] Zambia,
    [EnumMember(Value = "ZW")] Zimbabwe,
}

public sealed class CountryCodeConverter : JsonConverter<CountryCode>
{
    internal static readonly Dictionary<string, CountryCode> Map;

    static CountryCodeConverter()
    {
        Map = new Dictionary<string, CountryCode>(StringComparer.OrdinalIgnoreCase);
        foreach (CountryCode code in Enum.GetValues(typeof(CountryCode)))
        {
            var value = GetValue(code);
            Map[value] = code;
        }
    }

    internal static string GetValue(CountryCode code)
    {
        var member = typeof(CountryCode).GetMember(code.ToString())[0];
        var attr = (EnumMemberAttribute?)Attribute.GetCustomAttribute(member, typeof(EnumMemberAttribute));
        return attr?.Value ?? code.ToString();
    }

    public override CountryCode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var raw = reader.GetString();
        if (raw is null)
        {
            throw new JsonException("Country code was null");
        }

        string value = raw!;
        if (Map.TryGetValue(value, out var code))
        {
            return code;
        }

        throw new JsonException($"Invalid country code '{value}'");
    }

    public override void Write(Utf8JsonWriter writer, CountryCode value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(GetValue(value));
    }
}

public static class CountryCodeExtensions
{
    public static bool TryParse(string? value, out CountryCode code)
    {
        if (!string.IsNullOrEmpty(value))
        {
            if (CountryCodeConverter.Map.TryGetValue(value!, out code))
            {
                return true;
            }
            if (Enum.TryParse(value!, true, out code))
            {
                return true;
            }
        }
        code = default;
        return false;
    }

    public static string ToValueString(this CountryCode code) => CountryCodeConverter.GetValue(code);
}
