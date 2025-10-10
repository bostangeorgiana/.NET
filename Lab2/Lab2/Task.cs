/*
   record = tip special de clasa
            - e folosit pt date IMUTABILE (care nu se schimba)
            - se foloseste pt modele simple (ex DTO)
            - deja e implementat Equals(), ToString()
            - e mai usor sa clonezi obiecte
 */

public record Task(string Title, bool IsCompleted, DateTime DueDate);

/*
    with - cloneaza
        - poti copia un record + sa modifci o parte din el
*/