#define CONTRACTS_FULL
using System.Diagnostics.Contracts;
public class PatientVisit
{
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public double age { get; set; }
    public int cost_of_visit { get; set; } = 0;
}
[ContractClass(typeof(PatientContract))]
public interface IPatient
{
    double total_cost_of_visits { get; set; }
    void Add(PatientVisit visit);
    void Add2(PatientVisit[] visits);
}
[ContractClassFor(typeof(IPatient))]
public abstract class PatientContract : IPatient
{
    public abstract double total_cost_of_visits { get; set; }
    public void Add(PatientVisit visit)
    {
        Contract.Requires(visit != null, "visit can't be null");
        Contract.Requires(!string.IsNullOrEmpty(visit.Name), "visit name can't be blank");

        Contract.Requires(visit.Date <= DateTime.Now, "Date can't be future");

        Contract.Requires(visit.age > 0, "patient age can't be negative or zero");

        Contract.Requires(visit.cost_of_visit > 0, "cost of visit can't be negative or zero");
    }
    public void Add2(PatientVisit[] visits)
    {
        Contract.Requires(
            (visits != null) &&
            Contract.ForAll(visits, visit => visit != null), "visits can't be null");
    }
}
public class PatientProcessor : IPatient
{
    public double total_cost_of_visits { get; set; }

    public void Add(PatientVisit visit)
    {
        Contract.Requires(visit != null, "visit can't be null");
        Contract.Requires(!string.IsNullOrEmpty(visit.Name), "visit name can't be blank");

        Contract.Requires(visit.Date <= DateTime.Now, "Date can't be future");

        Contract.Requires(visit.age > 0, "patient age can't be negative or zero");

        Contract.Requires(visit.cost_of_visit > 0, "cost of visit can't be negative or zero");
        this.total_cost_of_visits += visit.cost_of_visit;
        Console.WriteLine("Patient registered has been registered a new visit!");
    }

    public void Add2(PatientVisit[] visits)
    {
        foreach (var visit in visits)
        {
            Contract.Requires(
                (visits != null) &&
                Contract.ForAll(visits, visit => visit != null), "visits can't be null");
            this.total_cost_of_visits += visit.cost_of_visit;
        }
    }
}
class Program
{
    static void Contract_ContractFailed(object sender, ContractFailedEventArgs e)
    {
        Console.WriteLine(e.Message);
        e.SetUnwind();
        e.SetHandled();
    }
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Contract.ContractFailed += new EventHandler<ContractFailedEventArgs>(Contract_ContractFailed);
        var patientProcessor = new PatientProcessor();
        patientProcessor.total_cost_of_visits = 0;
        var payment1 = new PatientVisit
        {
            Name = "qwe",
            age = 70,
            cost_of_visit = 101,
            Date = new DateTime(2023, 10, 15, 2, 29, 59)
        };
        patientProcessor.Add(payment1);
        patientProcessor.Add(null);
    }
}
// See https://aka.ms/new-console-template for more information
