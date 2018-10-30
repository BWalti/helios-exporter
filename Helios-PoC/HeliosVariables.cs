namespace Helios_PoC
{

    // see reference: https://www.easycontrols.net/en/service/downloads/send/4-software/16-modbus-dokumentation-f%C3%BCr-kwl-easycontrols-ger%C3%A4te
    public static class HeliosVariables
    {
        public static VariableDeclaration<float> FortluftTemperatur => new VariableDeclaration<float>(
            "v00106",
            8,
            "Temperatur: Fortluft",
            AccessMode.R,
            -27,
            9999);

        public static VariableDeclaration<float> AbluftTemperatur => new VariableDeclaration<float>(
            "v00107",
            8,
            "Temperatur: Abluft",
            AccessMode.R,
            -27,
            9999);

        public static VariableDeclaration<float> ZuluftTemperatur => new VariableDeclaration<float>(
            "v00105",
            8,
            "Temperatur: Zuluft",
            AccessMode.R,
            -27,
            9999);

        public static VariableDeclaration<float> AussenluftTemperatur => new VariableDeclaration<float>(
            "v00104",
            8,
            "Temperatur: Aussenluft",
            AccessMode.R,
            -27,
            9999);
        public static VariableDeclaration<string> Uhrzeit => new VariableDeclaration<string>(
            "v00005",
            9,
            "hh:mm:ss",
            AccessMode.RW);


        public static VariableDeclaration<float> SpannungAbluft1 => new VariableDeclaration<float>(
            "v00012",
            6,
            "Spannung Lüfterstufe 1: Abluft",
            AccessMode.RW,
            1.6f,
            10);

        public static VariableDeclaration<float> SpannungAbluft2 => new VariableDeclaration<float>(
            "v00014",
            6,
            "Spannung Lüfterstufe 2: Abluft",
            AccessMode.RW,
            1.6f,
            10);

        public static VariableDeclaration<float> SpannungAbluft3 => new VariableDeclaration<float>(
            "v00016",
            6,
            "Spannung Lüfterstufe 3: Abluft",
            AccessMode.RW,
            1.6f,
            10);

        public static VariableDeclaration<float> SpannungAbluft4 => new VariableDeclaration<float>(
            "v00018",
            6,
            "Spannung Lüfterstufe 4: Abluft",
            AccessMode.RW,
            1.6f,
            10);

        public static VariableDeclaration<float> SpannungZuluft1 => new VariableDeclaration<float>(
            "v00013",
            6,
            "Spannung Lüfterstufe 1: Zuluft",
            AccessMode.RW,
            1.6f,
            10);

        public static VariableDeclaration<float> SpannungZuluft2 => new VariableDeclaration<float>(
            "v00015",
            6,
            "Spannung Lüfterstufe 2: Zuluft",
            AccessMode.RW,
            1.6f,
            10);

        public static VariableDeclaration<float> SpannungZuluft3 => new VariableDeclaration<float>(
            "v00017",
            6,
            "Spannung Lüfterstufe 3: Zuluft",
            AccessMode.RW,
            1.6f,
            10);

        public static VariableDeclaration<float> SpannungZuluft4 => new VariableDeclaration<float>(
            "v00019",
            6,
            "Spannung Lüfterstufe 4: Zuluft",
            AccessMode.RW,
            1.6f,
            10);

        public static VariableDeclaration<ushort> MinLüfterstufe => new VariableDeclaration<ushort>(
            "v00020",
            6,
            "Minimale Lüfterstufe",
            AccessMode.RW,
            0,
            1);

        public static VariableDeclaration<ushort> Lüfterstufe => new VariableDeclaration<ushort>(
            "v00102",
            5,
            "Aktuelle Lüfterstufe",
            AccessMode.RW,
            0,
            4);

        public static VariableDeclaration<float> ProzentualeLüfterstufe => new VariableDeclaration<float>(
            "v00103",
            6,
            "Aktuelle Prozentuale Lüfterstufe",
            AccessMode.R,
            0,
            100);
    }
}