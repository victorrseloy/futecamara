/* Copyright 2013 de Victor Ribeiro da Silva Eloy Este arquivo é parte do programa FUTECAMARA. O FUTECAMARA é um software livre; você pode redistribuí-lo e/ou modificá-lo dentro dos termos da GNU General Public License como publicada pela Fundação do Software Livre (FSF); na versão 3 da Licença. Este programa é distribuído na esperança que possa ser útil, mas SEM NENHUMA GARANTIA; sem uma garantia implícita de ADEQUAÇÃO a qualquer MERCADO ou APLICAÇÃO EM PARTICULAR. Veja a licença para maiores detalhes. Você deve ter recebido uma cópia da GNU General Public License, sob o título "LICENSE", junto com este programa, se não, acesse http://www.gnu.org/licenses/ */

using System.Data.Entity;

namespace Futenado.Models
{
    public class EntityContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<Futenado.Models.EntityContext>());

        public EntityContext()
            : base("name=EntityContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Deputado> Deputadoes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Time> Times { get; set; }
        public DbSet<DeputadoTime> DeputadosTime { get; set; }
        public DbSet<Chave> Chaves { get; set; }
        public DbSet<Partida> Partidas { get; set; }
        

    }
}
