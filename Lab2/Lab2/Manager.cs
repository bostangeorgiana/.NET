/*
    init-only properties    - se pot seta doar la crearea obiectului
                            - dupa aceea, devin read-only.
                            - atunci cand vrei ca obiectele sa nu mai fie modificate dupaa eroare
 */
public class Manager
{
    public string Name { get; init; }
    public string Team { get; init; }
    public string Email { get; init; }
}