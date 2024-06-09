// See https://aka.ms/new-console-template for more information
public interface IObserver
{
    void Update(float temperature, float humidity, float pressure);
}

public interface IDisplayElement
{
    void Display();
}

public class WeatherStation
{
    private List<IObserver> observers;
    private float temperature;
    private float humidity;
    private float pressure;

    public WeatherStation()
    {
        observers = new List<IObserver>();
    }

    public void RegisterObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (IObserver observer in observers)
        {
            observer.Update(temperature, humidity, pressure);
        }
    }

    public void SetMeasurements(float temperature, float humidity, float pressure)
    {
        this.temperature = temperature;
        this.humidity = humidity;
        this.pressure = pressure;
        NotifyObservers();
    }
}


public class CurrentConditionsDisplay : IObserver, IDisplayElement
{
    private float temperature;
    private float humidity;
    private WeatherStation weatherStation;

    public CurrentConditionsDisplay(WeatherStation weatherStation)
    {
        this.weatherStation = weatherStation;
        weatherStation.RegisterObserver(this);
    }

    public void Update(float temperature, float humidity, float pressure)
    {
        this.temperature = temperature;
        this.humidity = humidity;
        Display();
    }

    public void Display()
    {
        Console.WriteLine($"Aktualne warunki: {temperature} stopni celcjusza i {humidity}% wilgotności");
    }
}


public class ForecastDisplay : IObserver, IDisplayElement
{
    private float currentPressure = 29.92f;
    private float lastPressure;
    private WeatherStation weatherStation;

    public ForecastDisplay(WeatherStation weatherStation)
    {
        this.weatherStation = weatherStation;
        weatherStation.RegisterObserver(this);
    }

    public void Update(float temperature, float humidity, float pressure)
    {
        lastPressure = currentPressure;
        currentPressure = pressure;
        Display();
    }

    public void Display()
    {
        Console.Write("Prognoza: ");
        if (currentPressure > lastPressure)
        {
            Console.WriteLine("Przewidywana poprawa warunków pogodowych");
        }
        else if (currentPressure == lastPressure)
        {
            Console.WriteLine("Brak perspektywn na poprawę warunków pogodowych");
        }
        else if (currentPressure < lastPressure)
        {
            Console.WriteLine("Spodziewane ochłodzenie i deszczowa pogoda");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        WeatherStation weatherStation = new WeatherStation();

        CurrentConditionsDisplay currentDisplay = new CurrentConditionsDisplay(weatherStation);
        ForecastDisplay forecastDisplay = new ForecastDisplay(weatherStation);

        weatherStation.SetMeasurements(33, 65, 30.4f);
        weatherStation.SetMeasurements(28, 70, 29.2f);
        weatherStation.SetMeasurements(29, 90, 29.2f);
    }
}

