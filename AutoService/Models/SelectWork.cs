namespace AutoService.Models;

public class SelectWork
{
    public Work Work { get; set; }
    public bool IsSelected { get; set; }

    public SelectWork(Work work)
    {
        Work = work;
    }
}