// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable StyleCop.SA1600
// ReSharper disable StringLiteralTypo

using Helios.Modbus.Internals;

namespace Helios.Modbus;

// see reference: https://www.easycontrols.net/en/service/downloads/send/4-software/16-modbus-dokumentation-f%C3%BCr-kwl-easycontrols-ger%C3%A4te
public static class HeliosParameters
{
    public static HeliosParameter<uint> AbgegebeneLeistungNachheizung { get; } = new(
        "v01109",
        9,
        "Abgegebene Leistung Nachheizung in %",
        AccessMode.R,
        0,
        uint.MaxValue);

    public static HeliosParameter<uint> AbgegebeneLeistungVorheizung { get; } = new(
        "v01108",
        9,
        "Abgegebene Leistung Vorheizung in %",
        AccessMode.R,
        0,
        uint.MaxValue);

    public static HeliosParameter<ushort> AbluftLüfterstufe { get; } = new(
        "v01051",
        5,
        "Aktuelle Abluft Luefterstufe",
        AccessMode.RW,
        0,
        4);

    public static HeliosParameter<ushort> AbluftRpm { get; } =
        new("v00349", 6, "Abluft min-1", AccessMode.R, 0, 9999);

    public static HeliosParameter<float> AbluftTemperatur { get; } = new(
        "v00107",
        8,
        "Temperatur: Abluft",
        AccessMode.R,
        -27,
        9999);

    public static HeliosParameter<float> AussenluftTemperatur { get; } = new(
        "v00104",
        8,
        "Temperatur: Aussenluft",
        AccessMode.R,
        -27,
        9999);

    public static HeliosParameter<float> BehaglichkeitsTemperatur { get; } = new(
        "v00043",
        8,
        "Temperatur: Behaglichkeit",
        AccessMode.R,
        10,
        25);

    public static HeliosParameter<ushort> Betriebsmodus { get; } = new(
        "v00101",
        5,
        "0 = Auto, 1 = Manuell",
        AccessMode.RW,
        0,
        1);

    public static HeliosParameter<uint> BetriebsZeitAbluftVentilator { get; } = new(
        "v01104",
        9,
        "Betriebs Stunden Abluft Ventilator in Minuten",
        AccessMode.R,
        0,
        uint.MaxValue);

    public static HeliosParameter<uint> BetriebsZeitNachheizung { get; } = new(
        "v01106",
        9,
        "Betriebs Stunden Nachheizung in Minuten",
        AccessMode.R,
        0,
        uint.MaxValue);

    public static HeliosParameter<uint> BetriebsZeitVorheizung { get; } = new(
        "v01105",
        9,
        "Betriebs Stunden Vorheizung in Minuten",
        AccessMode.R,
        0,
        uint.MaxValue);

    public static HeliosParameter<uint> BetriebsZeitZuluftVentilator { get; } = new(
        "v01103",
        9,
        "Betriebs Stunden Zuluft Ventilator in Minuten",
        AccessMode.R,
        0,
        uint.MaxValue);

    public static HeliosParameter<ushort> BypassMinAussentemperaturTemperatur { get; } =
        new("v01036", 5, "in Grad Celsius", AccessMode.RW, 5, 20);

    public static HeliosParameter<ushort> BypassRaumTemperatur { get; } = new(
        "v01035",
        5,
        "in Grad Celsius",
        AccessMode.RW,
        10,
        40);

    public static HeliosParameter<float> FortluftTemperatur { get; } = new(
        "v00106",
        8,
        "Temperatur: Fortluft",
        AccessMode.R,
        -27,
        9999);

    public static HeliosParameter<ushort> Luefterstufe { get; } = new(
        "v00102",
        5,
        "Aktuelle Luefterstufe",
        AccessMode.RW,
        0,
        4);

    public static HeliosParameter<ushort> MinLüfterstufe { get; } = new(
        "v00020",
        6,
        "Minimale Luefterstufe",
        AccessMode.RW,
        0,
        1);

    public static HeliosParameter<ushort> PartyBetrieb { get; } = new(
        "v00094",
        5,
        "Partybetrieb 0 = Aus, 1 = Ein",
        AccessMode.RW,
        0,
        1);

    public static HeliosParameter<ushort> PartyDauer { get; } = new(
        "v00091",
        6,
        "Partybetrieb Dauer in Minuten",
        AccessMode.RW,
        5,
        180);

    public static HeliosParameter<ushort> PartyLüfterstufe { get; } = new(
        "v00092",
        5,
        "Partybetrieb Luefterstufe",
        AccessMode.RW,
        0,
        4);

    public static HeliosParameter<ushort> PartyRestlaufzeit { get; } = new(
        "v00093",
        6,
        "Partybetrieb Restlaufzeit in Minuten",
        AccessMode.R,
        0,
        180);

    public static HeliosParameter<float> ProzentualeLuefterstufe { get; } = new(
        "v00103",
        6,
        "Aktuelle Prozentuale Luefterstufe",
        AccessMode.R,
        0,
        100);

    public static HeliosParameter<uint> Restlaufzeit { get; } = new(
        "v01033",
        9,
        "Restlaufzeit (Filter) in Minuten",
        AccessMode.R,
        0,
        uint.MaxValue);

    public static HeliosParameter<ushort> RuhebetriebBetrieb { get; } = new(
        "v00099",
        5,
        "Ruhebetrieb 0 = Aus, 1 = Ein",
        AccessMode.RW,
        0,
        1);

    public static HeliosParameter<ushort> RuhebetriebDauer { get; } = new(
        "v00096",
        6,
        "Ruhebetrieb Dauer in Minuten",
        AccessMode.RW,
        5,
        180);

    public static HeliosParameter<ushort> RuhebetriebLüfterstufe { get; } = new(
        "v00097",
        5,
        "Ruhebetrieb Luefterstufe",
        AccessMode.RW,
        0,
        4);

    public static HeliosParameter<ushort> RuhebetriebRestlaufzeit { get; } = new(
        "v00098",
        6,
        "Partybetrieb Restlaufzeit in Minuten",
        AccessMode.R,
        0,
        180);

    public static HeliosParameter<float> SpannungAbluft1 { get; } = new(
        "v00012",
        6,
        "Spannung Luefterstufe 1: Abluft",
        AccessMode.RW,
        1.6f,
        10);

    public static HeliosParameter<float> SpannungAbluft2 { get; } = new(
        "v00014",
        6,
        "Spannung Luefterstufe 2: Abluft",
        AccessMode.RW,
        1.6f,
        10);

    public static HeliosParameter<float> SpannungAbluft3 { get; } = new(
        "v00016",
        6,
        "Spannung Luefterstufe 3: Abluft",
        AccessMode.RW,
        1.6f,
        10);

    public static HeliosParameter<float> SpannungAbluft4 { get; } = new(
        "v00018",
        6,
        "Spannung Luefterstufe 4: Abluft",
        AccessMode.RW,
        1.6f,
        10);

    public static HeliosParameter<float> SpannungZuluft1 { get; } = new(
        "v00013",
        6,
        "Spannung Luefterstufe 1: Zuluft",
        AccessMode.RW,
        1.6f,
        10);

    public static HeliosParameter<float> SpannungZuluft2 { get; } = new(
        "v00015",
        6,
        "Spannung Luefterstufe 2: Zuluft",
        AccessMode.RW,
        1.6f,
        10);

    public static HeliosParameter<float> SpannungZuluft3 { get; } = new(
        "v00017",
        6,
        "Spannung Luefterstufe 3: Zuluft",
        AccessMode.RW,
        1.6f,
        10);

    public static HeliosParameter<float> SpannungZuluft4 { get; } = new(
        "v00019",
        6,
        "Spannung Luefterstufe 4: Zuluft",
        AccessMode.RW,
        1.6f,
        10);

    public static HeliosParameter<string> Uhrzeit { get; } = new("v00005", 9, "hh:mm:ss", AccessMode.RW);

    public static HeliosParameter<ushort> VorheizungStatus { get; } = new(
        "v00024",
        5,
        "0 = Aus, 1 = Ein",
        AccessMode.RW,
        0,
        1);

    public static HeliosParameter<ushort> Wechselintervall { get; } = new(
        "v01032",
        5,
        "Wechselintervall in Monaten",
        AccessMode.RW,
        0,
        12);

    public static HeliosParameter<ushort> ZuluftLüfterstufe { get; } = new(
        "v01050",
        5,
        "Aktuelle Zuluft Luefterstufe",
        AccessMode.RW,
        0,
        4);

    public static HeliosParameter<ushort> ZuluftRpm { get; } =
        new("v00348", 6, "Zuluft min-1", AccessMode.R, 0, 9999);

    public static HeliosParameter<float> ZuluftTemperatur { get; } = new(
        "v00105",
        8,
        "Temperatur: Zuluft",
        AccessMode.R,
        -27,
        9999);
}