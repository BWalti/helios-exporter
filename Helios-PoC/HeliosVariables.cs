// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable StyleCop.SA1600
// ReSharper disable StringLiteralTypo

namespace Helios_PoC
{
    // see reference: https://www.easycontrols.net/en/service/downloads/send/4-software/16-modbus-dokumentation-f%C3%BCr-kwl-easycontrols-ger%C3%A4te
    public static class HeliosVariables
    {
        public static VariableDeclaration<uint> AbgegebeneLeistungNachheizung { get; } = new VariableDeclaration<uint>(
            "v01109",
            9,
            "Abgegebene Leistung Nachheizung in %",
            AccessMode.R,
            0,
            uint.MaxValue);

        public static VariableDeclaration<uint> AbgegebeneLeistungVorheizung { get; } = new VariableDeclaration<uint>(
            "v01108",
            9,
            "Abgegebene Leistung Vorheizung in %",
            AccessMode.R,
            0,
            uint.MaxValue);

        public static VariableDeclaration<ushort> AbluftLüfterstufe { get; } = new VariableDeclaration<ushort>(
            "v01051",
            5,
            "Aktuelle Abluft Lüfterstufe",
            AccessMode.RW,
            0,
            4);

        public static VariableDeclaration<ushort> AbluftRpm { get; } =
            new VariableDeclaration<ushort>("v00349", 6, "Abluft min-1", AccessMode.R, 0, 9999);

        public static VariableDeclaration<float> AbluftTemperatur { get; } = new VariableDeclaration<float>(
            "v00107",
            8,
            "Temperatur: Abluft",
            AccessMode.R,
            -27,
            9999);

        public static VariableDeclaration<float> AussenluftTemperatur { get; } = new VariableDeclaration<float>(
            "v00104",
            8,
            "Temperatur: Aussenluft",
            AccessMode.R,
            -27,
            9999);

        public static VariableDeclaration<float> BehaglichkeitsTemperatur { get; } = new VariableDeclaration<float>(
            "v00043",
            8,
            "Temperatur: Behaglichkeit",
            AccessMode.R,
            10,
            25);

        public static VariableDeclaration<ushort> Betriebsmodus { get; } = new VariableDeclaration<ushort>(
            "v00101",
            5,
            "0 = Auto, 1 = Manuell",
            AccessMode.RW,
            0,
            1);

        public static VariableDeclaration<uint> BetriebsZeitAbluftVentilator { get; } = new VariableDeclaration<uint>(
            "v01104",
            9,
            "Betriebs Stunden Abluft Ventilator in Minuten",
            AccessMode.R,
            0,
            uint.MaxValue);

        public static VariableDeclaration<uint> BetriebsZeitNachheizung { get; } = new VariableDeclaration<uint>(
            "v01106",
            9,
            "Betriebs Stunden Nachheizung in Minuten",
            AccessMode.R,
            0,
            uint.MaxValue);

        public static VariableDeclaration<uint> BetriebsZeitVorheizung { get; } = new VariableDeclaration<uint>(
            "v01105",
            9,
            "Betriebs Stunden Vorheizung in Minuten",
            AccessMode.R,
            0,
            uint.MaxValue);

        public static VariableDeclaration<uint> BetriebsZeitZuluftVentilator { get; } = new VariableDeclaration<uint>(
            "v01103",
            9,
            "Betriebs Stunden Zuluft Ventilator in Minuten",
            AccessMode.R,
            0,
            uint.MaxValue);

        public static VariableDeclaration<ushort> BypassMinAussentemperaturTemperatur { get; } =
            new VariableDeclaration<ushort>("v01036", 5, "in Grad Celsius", AccessMode.RW, 5, 20);

        public static VariableDeclaration<ushort> BypassRaumTemperatur { get; } = new VariableDeclaration<ushort>(
            "v01035",
            5,
            "in Grad Celsius",
            AccessMode.RW,
            10,
            40);

        public static VariableDeclaration<float> FortluftTemperatur { get; } = new VariableDeclaration<float>(
            "v00106",
            8,
            "Temperatur: Fortluft",
            AccessMode.R,
            -27,
            9999);

        public static VariableDeclaration<ushort> Lüfterstufe { get; } = new VariableDeclaration<ushort>(
            "v00102",
            5,
            "Aktuelle Lüfterstufe",
            AccessMode.RW,
            0,
            4);

        public static VariableDeclaration<ushort> MinLüfterstufe { get; } = new VariableDeclaration<ushort>(
            "v00020",
            6,
            "Minimale Lüfterstufe",
            AccessMode.RW,
            0,
            1);

        public static VariableDeclaration<ushort> PartyBetrieb { get; } = new VariableDeclaration<ushort>(
            "v00094",
            5,
            "Partybetrieb 0 = Aus, 1 = Ein",
            AccessMode.RW,
            0,
            1);

        public static VariableDeclaration<ushort> PartyDauer { get; } = new VariableDeclaration<ushort>(
            "v00091",
            6,
            "Partybetrieb Dauer in Minuten",
            AccessMode.RW,
            5,
            180);

        public static VariableDeclaration<ushort> PartyLüfterstufe { get; } = new VariableDeclaration<ushort>(
            "v00092",
            5,
            "Partybetrieb Lüfterstufe",
            AccessMode.RW,
            0,
            4);

        public static VariableDeclaration<ushort> PartyRestlaufzeit { get; } = new VariableDeclaration<ushort>(
            "v00093",
            6,
            "Partybetrieb Restlaufzeit in Minuten",
            AccessMode.R,
            0,
            180);

        public static VariableDeclaration<float> ProzentualeLüfterstufe { get; } = new VariableDeclaration<float>(
            "v00103",
            6,
            "Aktuelle Prozentuale Lüfterstufe",
            AccessMode.R,
            0,
            100);

        public static VariableDeclaration<uint> Restlaufzeit { get; } = new VariableDeclaration<uint>(
            "v01033",
            9,
            "Restlaufzeit (Filter) in Minuten",
            AccessMode.R,
            0,
            uint.MaxValue);

        public static VariableDeclaration<ushort> RuhebetriebBetrieb { get; } = new VariableDeclaration<ushort>(
            "v00099",
            5,
            "Ruhebetrieb 0 = Aus, 1 = Ein",
            AccessMode.RW,
            0,
            1);

        public static VariableDeclaration<ushort> RuhebetriebDauer { get; } = new VariableDeclaration<ushort>(
            "v00096",
            6,
            "Ruhebetrieb Dauer in Minuten",
            AccessMode.RW,
            5,
            180);

        public static VariableDeclaration<ushort> RuhebetriebLüfterstufe { get; } = new VariableDeclaration<ushort>(
            "v00097",
            5,
            "Ruhebetrieb Lüfterstufe",
            AccessMode.RW,
            0,
            4);

        public static VariableDeclaration<ushort> RuhebetriebRestlaufzeit { get; } = new VariableDeclaration<ushort>(
            "v00098",
            6,
            "Partybetrieb Restlaufzeit in Minuten",
            AccessMode.R,
            0,
            180);

        public static VariableDeclaration<float> SpannungAbluft1 { get; } = new VariableDeclaration<float>(
            "v00012",
            6,
            "Spannung Lüfterstufe 1: Abluft",
            AccessMode.RW,
            1.6f,
            10);

        public static VariableDeclaration<float> SpannungAbluft2 { get; } = new VariableDeclaration<float>(
            "v00014",
            6,
            "Spannung Lüfterstufe 2: Abluft",
            AccessMode.RW,
            1.6f,
            10);

        public static VariableDeclaration<float> SpannungAbluft3 { get; } = new VariableDeclaration<float>(
            "v00016",
            6,
            "Spannung Lüfterstufe 3: Abluft",
            AccessMode.RW,
            1.6f,
            10);

        public static VariableDeclaration<float> SpannungAbluft4 { get; } = new VariableDeclaration<float>(
            "v00018",
            6,
            "Spannung Lüfterstufe 4: Abluft",
            AccessMode.RW,
            1.6f,
            10);

        public static VariableDeclaration<float> SpannungZuluft1 { get; } = new VariableDeclaration<float>(
            "v00013",
            6,
            "Spannung Lüfterstufe 1: Zuluft",
            AccessMode.RW,
            1.6f,
            10);

        public static VariableDeclaration<float> SpannungZuluft2 { get; } = new VariableDeclaration<float>(
            "v00015",
            6,
            "Spannung Lüfterstufe 2: Zuluft",
            AccessMode.RW,
            1.6f,
            10);

        public static VariableDeclaration<float> SpannungZuluft3 { get; } = new VariableDeclaration<float>(
            "v00017",
            6,
            "Spannung Lüfterstufe 3: Zuluft",
            AccessMode.RW,
            1.6f,
            10);

        public static VariableDeclaration<float> SpannungZuluft4 { get; } = new VariableDeclaration<float>(
            "v00019",
            6,
            "Spannung Lüfterstufe 4: Zuluft",
            AccessMode.RW,
            1.6f,
            10);

        public static VariableDeclaration<string> Uhrzeit { get; } =
            new VariableDeclaration<string>("v00005", 9, "hh:mm:ss", AccessMode.RW);

        public static VariableDeclaration<ushort> VorheizungStatus { get; } = new VariableDeclaration<ushort>(
            "v00024",
            5,
            "0 = Aus, 1 = Ein",
            AccessMode.RW,
            0,
            1);

        public static VariableDeclaration<ushort> Wechselintervall { get; } = new VariableDeclaration<ushort>(
            "v01032",
            5,
            "Wechselintervall in Monaten",
            AccessMode.RW,
            0,
            12);

        public static VariableDeclaration<ushort> ZuluftLüfterstufe { get; } = new VariableDeclaration<ushort>(
            "v01050",
            5,
            "Aktuelle Zuluft Lüfterstufe",
            AccessMode.RW,
            0,
            4);

        public static VariableDeclaration<ushort> ZuluftRpm { get; } =
            new VariableDeclaration<ushort>("v00348", 6, "Zuluft min-1", AccessMode.R, 0, 9999);

        public static VariableDeclaration<float> ZuluftTemperatur { get; } = new VariableDeclaration<float>(
            "v00105",
            8,
            "Temperatur: Zuluft",
            AccessMode.R,
            -27,
            9999);
    }
}