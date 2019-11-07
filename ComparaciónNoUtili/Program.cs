using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComparaciónNoUtili
{
    class Program
    {
        static void comparacionNoUtili()
        {
            ArrendamientoInmuebleEntities ctx = new ArrendamientoInmuebleEntities();
            var tablaTest = ctx.NoUtiliAuditoria2019T;
            var tablaProd = ctx.NoUtiliAuditoria2019TProduccion;
            foreach (var test in tablaTest)
            {
                Console.WriteLine("/" + test.FolioContratoArrto + "/");
                var result = (from prod in tablaProd
                              where test.FolioContratoArrto == prod.FolioContratoArrto
                              select new
                              {
                                  FolioContrato = test.FolioContratoArrto,
                                  IDInmueble = test.IdInmuebleArrendamiento,
                                  IDInmuebleProd = prod.IdInmuebleArrendamiento,
                                  Inst = test.Fk_IdInstitucion,
                                  InstProd = prod.Fk_IdInstitucion,
                                  FechaFin = test.FechaFinOcupacion,
                                  FechaFinProd = prod.FechaFinOcupacion
                              });
                foreach (var r in result)
                {
                    DateTime fecha = r.FechaFin != null ? (DateTime)r.FechaFin :
                    new DateTime(1990, 1, 1);
                    DateTime fechaProd = r.FechaFinProd != null ? (DateTime)r.FechaFinProd :
                    new DateTime(1990, 1, 1);
                    Console.WriteLine("-" + r.IDInmueble + "-" + r.IDInmuebleProd + "-" +
                        r.Inst + "-" + r.InstProd + "-" +
                        fecha + "-" + fechaProd);
                }
            }
        }

        static void BusquedaVigentes()
        {
            ArrendamientoInmuebleEntities ctx = new ArrendamientoInmuebleEntities();
            var Contartos = ctx.ContratoArrto;
            var Vigentes = ctx.ContratosVigentes;
        }

        static void Main(string[] args)
        {
            //comparacionNoUtili();
            BusquedaVigentes();

            Console.ReadLine();
        }
    }
}

/*
            foreach(NoUtiliAuditoria2019T renglonTest in tablaTest)
            {
                DateTime fecha = renglonTest.FechaFinOcupacion != null ? (DateTime) renglonTest.FechaFinOcupacion :
                    new DateTime(1990,1,1);
                Console.WriteLine("Test " + renglonTest.FolioContratoArrto + " " +
                                            renglonTest.IdInmuebleArrendamiento + " " +
                                            renglonTest.Fk_IdInstitucion + " " +
                                            fecha.ToString("yyyy MMMM dd"));
                
            }
            */
