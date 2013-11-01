///* Copyright 2013 de Victor Ribeiro da Silva Eloy Este arquivo � parte do programa FUTECAMARA. O FUTECAMARA � um software livre; voc� pode redistribu�-lo e/ou modific�-lo dentro dos termos da GNU General Public License como publicada pela Funda��o do Software Livre (FSF); na vers�o 3 da Licen�a. Este programa � distribu�do na esperan�a que possa ser �til, mas SEM NENHUMA GARANTIA; sem uma garantia impl�cita de ADEQUA��O a qualquer MERCADO ou APLICA��O EM PARTICULAR. Veja a licen�a para maiores detalhes. Voc� deve ter recebido uma c�pia da GNU General Public License, sob o t�tulo "LICENSE", junto com este programa, se n�o, acesse http://www.gnu.org/licenses/ */

namespace Futenado.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Futenado.Models.EntityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Futenado.Models.EntityContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
