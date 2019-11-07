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
            var Contratos = ctx.ContratoArrto;
            var Vigentes = ctx.ContratosVigentes;
            var NoUtili = ctx.NoUtiliAuditoria2019T;

            foreach (var vigente in Vigentes)
            {
                // result contiene los contratos vigentes que estan en la base cuyo inmueble lo podemos encontrar
                // aun es necesaro verificar que los datos del inmueble son corretos
                var result = (from contrato in Contratos
                              join relacionInv in NoUtili
                              on contrato.Fk_IdInmuebleArrendamiento equals relacionInv.IdInmuebleArrendamiento
                              where vigente.FolioContrato == contrato.FolioContratoArrto
                              select new
                              {
                                  FolioContrato = contrato.FolioContratoArrto,
                                  IdContrato    = contrato.IdContratoArrto,
                                  IdInmueble = contrato.Fk_IdInmuebleArrendamiento,
                                  Inst = contrato.Fk_IdInstitucion,
                                  FechaFin = contrato.FechaFinOcupacion,
                                  FechaFinVigente = vigente.FechaContratoHasta
                              });
                int numeroRegistros = result.Count();
                if (numeroRegistros > 0)
                {
                    Console.WriteLine("C /" + vigente.FolioContrato + "/ " + numeroRegistros);
                    foreach (var r in result)
                    {
                        DateTime fecha = r.FechaFin != null ? (DateTime)r.FechaFin :
                        new DateTime(1990, 1, 1);
                        DateTime fechaVigente = r.FechaFinVigente != null ? (DateTime)r.FechaFinVigente :
                        new DateTime(1990, 1, 1);
                        Console.WriteLine("-" + r.FolioContrato + "-" + r.IdContrato + "-" +
                            r.IdInmueble + "-" +
                            r.Inst + "-" +
                            fecha + "-" + fechaVigente);
                    }
                }
                else
                {
                    // incompleto contiene los contratos vigentes que estan en la base y no tienen un inmueble localizable 
                    var incompleto = (from contrato in Contratos
                                  where vigente.FolioContrato == contrato.FolioContratoArrto
                                  select new
                                  {
                                      FolioContrato = contrato.FolioContratoArrto,
                                      IdContrato = contrato.IdContratoArrto,
                                      Inst = contrato.Fk_IdInstitucion,
                                      FechaFin = contrato.FechaFinOcupacion,
                                      FechaFinVigente = vigente.FechaContratoHasta
                                  });
                    Console.WriteLine("I /" + vigente.FolioContrato + "/ " + numeroRegistros);
                    foreach (var r in incompleto)
                    {
                        DateTime fecha = r.FechaFin != null ? (DateTime)r.FechaFin :
                        new DateTime(1990, 1, 1);
                        DateTime fechaVigente = r.FechaFinVigente != null ? (DateTime)r.FechaFinVigente :
                        new DateTime(1990, 1, 1);
                        Console.WriteLine("-" + r.FolioContrato + "-" + r.IdContrato + "-" +
                            r.Inst + "-" +
                            fecha + "-" + fechaVigente);
                        var contXIDEncontrato = (from relacionInv in NoUtili
                                              where r.IdContrato == relacionInv.FolioContratoArrto
                                              select relacionInv);
                        Console.WriteLine("Datos encontrados para verificar");
                        foreach(var rIDE in contXIDEncontrato)
                        {
                            DateTime fechaID = rIDE.FechaFinOcupacion != null ? (DateTime)rIDE.FechaFinOcupacion :
                                new DateTime(1990, 1, 1);
                            Console.WriteLine("-FolioContrato " + rIDE.FolioContratoArrto + "-" + fechaID + "-" +
                                rIDE.IdInmuebleArrendamiento + "-" + rIDE.Fk_IdInstitucion);
                        }
                        
                    }
                    Console.ReadKey();
                } 
            }
        }

        static void Main(string[] args)
        {
            //comparacionNoUtili();
            BusquedaVigentes();

            /******  JUST TO SEE THE END   ****/
            Console.WriteLine("F I N");
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
