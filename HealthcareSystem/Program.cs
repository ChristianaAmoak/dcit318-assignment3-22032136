public class Repository<T>
{
    private readonly List<T> items = new();

    public void Add(T item)
    {
        items.Add(item);
    }

    public List<T> GetAll()
    {
        return items;
    }

    public T? GetById(Func<T, bool> predicate)
    {
        return items.FirstOrDefault(predicate);
    }

    public bool Remove(Func<T, bool> predicate)
    {
        T? item = items.FirstOrDefault(predicate);

        if (item == null)
        {
            return false;
        }

        return items.Remove(item);
    }
}

public class Patient
{
    public int Id { get; }
    public string Name { get; }
    public int Age { get; }
    public string Gender { get; }

    public Patient(int id, string name, int age, string gender)
    {
        Id = id;
        Name = name;
        Age = age;
        Gender = gender;
    }
}

public class Prescription
{
    public int Id { get; }
    public int PatientId { get; }
    public string MedicationName { get; }
    public DateTime DateIssued { get; }

    public Prescription(
        int id,
        int patientId,
        string medicationName,
        DateTime dateIssued
    )
    {
        Id = id;
        PatientId = patientId;
        MedicationName = medicationName;
        DateIssued = dateIssued;
    }
}

public class HealthSystemApp
{
    private readonly Repository<Patient> _patientRepo = new();
    private readonly Repository<Prescription> _prescriptionRepo = new();

    private readonly Dictionary<int, List<Prescription>>
        _prescriptionMap = new();

    public void SeedData()
    {
        _patientRepo.Add(new Patient(1, "Kwame Mensah", 28, "Male"));
        _patientRepo.Add(new Patient(2, "Ama Boateng", 35, "Female"));
        _patientRepo.Add(new Patient(3, "Kojo Asare", 42, "Male"));

        _prescriptionRepo.Add(
            new Prescription(1, 1, "Paracetamol", DateTime.Now)
        );

        _prescriptionRepo.Add(
            new Prescription(2, 1, "Vitamin C", DateTime.Now)
        );

        _prescriptionRepo.Add(
            new Prescription(3, 2, "Amoxicillin", DateTime.Now)
        );

        _prescriptionRepo.Add(
            new Prescription(4, 2, "Ibuprofen", DateTime.Now)
        );

        _prescriptionRepo.Add(
            new Prescription(5, 3, "Aspirin", DateTime.Now)
        );
    }

    public void BuildPrescriptionMap()
    {
        foreach (Prescription prescription in _prescriptionRepo.GetAll())
        {
            if (!_prescriptionMap.ContainsKey(prescription.PatientId))
            {
                _prescriptionMap[prescription.PatientId] =
                    new List<Prescription>();
            }

            _prescriptionMap[prescription.PatientId].Add(prescription);
        }
    }

    public List<Prescription> GetPrescriptionsByPatientId(int patientId)
    {
        if (_prescriptionMap.ContainsKey(patientId))
        {
            return _prescriptionMap[patientId];
        }

        return new List<Prescription>();
    }

    public void PrintAllPatients()
    {
        Console.WriteLine("Patients:");

        foreach (Patient patient in _patientRepo.GetAll())
        {
            Console.WriteLine(
                $"ID: {patient.Id}, Name: {patient.Name}, Age: {patient.Age}, Gender: {patient.Gender}"
            );
        }
    }

    public void PrintPrescriptionsForPatient(int id)
    {
        Console.WriteLine($"\nPrescriptions for Patient ID {id}:");

        List<Prescription> prescriptions =
            GetPrescriptionsByPatientId(id);

        foreach (Prescription prescription in prescriptions)
        {
            Console.WriteLine(
                $"Prescription ID: {prescription.Id}, Medication: {prescription.MedicationName}, Date: {prescription.DateIssued:d}"
            );
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        HealthSystemApp app = new HealthSystemApp();

        app.SeedData();
        app.BuildPrescriptionMap();
        app.PrintAllPatients();
        app.PrintPrescriptionsForPatient(1);
    }
}
