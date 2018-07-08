using FluentNHibernate.Mapping;

namespace MyNotepad
{
    public class NoteMap : ClassMap<Note>
    {
        public NoteMap()
        {
            Id(x => x.id);
            Map(x => x.lastName);
            Map(x => x.firstName);
            Map(x => x.fathersName);
            Map(x => x.phoneNumber);
            Map(x => x.email);
            Map(x => x.birthDate);
            Table("notes");
        }
    }
}
