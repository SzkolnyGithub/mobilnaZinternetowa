using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace mobilnaZnodeJsBadowski4c;

public partial class MainPage : ContentPage
{
    List<Stacja> stacje = new List<Stacja>();
    public ObservableCollection<Stacja> listaStacji { get; set; }
	public class Stacja
	{
        public string id_stacji { get; set; }
        public string stacja { get; set; }
        public string data_pomiaru { get; set; }
        public string godzina_pomiaru { get; set; }
        public string temperatura { get; set; }
        public string predkosc_wiatru { get; set; }
        public string kierunek_wiatru { get; set; }
        public string wilgotnosc_wzgledna { get; set; }
        public string suma_opadu { get; set; }
        public string cisnienie { get; set; }
    }
    public async void GetPogoda()
    {
        HttpClient _client = new HttpClient();
        HttpResponseMessage response = await _client.GetAsync("http://10.0.2.2:3000/dane");
        var contents = await response.Content.ReadAsStringAsync();
        stacje = JsonConvert.DeserializeObject<List<Stacja>>(contents);
        Dane_label.Text = "Dane z serwisu zostały pobrane";
    }
    private void onEntryCompleted(object sender, EventArgs e)
    {
        var jest = false;
        foreach(var entry in stacje)
        {
            if(entry.id_stacji == Identyfikator.Text)
            {
                DisplayAlert("Informacje", "Stacja: " + entry.stacja + "\nTemperatura: " + entry.temperatura + "\nCisnienie: " + entry.cisnienie, "OK");
                jest = true;
            }
        }
        if (!jest)
        {
            DisplayAlert("Informacja", "Nie odnaleziono stacji o id = " + Identyfikator.Text, "OK");
        }
    }

    private void wybrany(object sender, SelectedItemChangedEventArgs e)
    {
        string idStacji = (e.SelectedItem as Stacja).id_stacji;
        string nazwa = (e.SelectedItem as Stacja).stacja;
        DisplayAlert("Informacje", "Stacja: " + nazwa + "\nId stacji: " + idStacji + "\nData pomiaru: " + (e.SelectedItem as Stacja).data_pomiaru, "OK");
    }

    private void dodaj(object sender, EventArgs e)
    {
        for (int i = 0; i < stacje.Count; i++)
        {
            listaStacji.Add(new Stacja
            {
                id_stacji = stacje[i].id_stacji,
                stacja = stacje[i].stacja,
                data_pomiaru = stacje[i].data_pomiaru,
                godzina_pomiaru = stacje[i].godzina_pomiaru,
                temperatura = stacje[i].temperatura,
                predkosc_wiatru = stacje[i].predkosc_wiatru,
                kierunek_wiatru = stacje[i].kierunek_wiatru,
                wilgotnosc_wzgledna = stacje[i].wilgotnosc_wzgledna,
                suma_opadu = stacje[i].suma_opadu,
                cisnienie = stacje[i].cisnienie
            });
        }
    }

	public MainPage()
	{
		InitializeComponent();
         
        GetPogoda();

        listaStacji = new ObservableCollection<Stacja> { };
        BindingContext = this;
	}

}

