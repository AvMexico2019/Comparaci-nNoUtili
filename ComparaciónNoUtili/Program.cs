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

        static string SinAcentos(string IN)
        {
            return IN.Replace("á", "a")
                .Replace("é", "e")
                .Replace("í", "i")
                .Replace("ó", "o")
                .Replace("ú", "u")
                .Replace("Á", "A")
                .Replace("É", "E")
                .Replace("Í", "I")
                .Replace("Ó", "O")
                .Replace("Ú", "U");
        }

        static void BusquedaVigentes()
        {
            int ContratosSinInmueble = 0;
            ArrendamientoInmuebleEntities ctx = new ArrendamientoInmuebleEntities();
            var Contratos = ctx.ContratoArrto;
            var Vigentes = ctx.ContratosVigentes;
            var NoUtili = ctx.NoUtiliAuditoria2019T;
            var inmuebles = ctx.InmuebleArrendamiento;

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
                   //Console.WriteLine("C /" + vigente.FolioContrato + "/ " + numeroRegistros);
                    foreach (var r in result)
                    {
                        DateTime fecha = r.FechaFin != null ? (DateTime)r.FechaFin :
                        new DateTime(1990, 1, 1);
                        DateTime fechaVigente = r.FechaFinVigente != null ? (DateTime)r.FechaFinVigente :
                        new DateTime(1990, 1, 1);
                        //Console.WriteLine("-" + r.FolioContrato + "-" + r.IdContrato + "-" +
                        //    r.IdInmueble + "-" +
                        //    r.Inst + "-" +
                        //    fecha + "-" + fechaVigente);
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
                    Console.WriteLine("\nI /" + vigente.FolioContrato + "/ " + numeroRegistros + "-" + vigente.Calle + "-" + vigente.NumExterior + " " +
                        vigente.CodigoPostal);
                    foreach (var r in incompleto)
                    {
                        DateTime fecha = r.FechaFin != null ? (DateTime)r.FechaFin :
                        new DateTime(1990, 1, 1);
                        DateTime fechaVigente = r.FechaFinVigente != null ? (DateTime)r.FechaFinVigente :
                        new DateTime(1990, 1, 1);
                        Console.WriteLine("-" + r.FolioContrato + "-" + r.IdContrato + "-" +
                            r.Inst + "-" +
                            fecha + "-" + fechaVigente);
                        /*
                        // Existia la posibilidad de que el numero de folio de contrato fuera un IdContrato, pero en muchos casos
                        // No apunta a ningún lado con información válida
                        var contXIDEncontrato = (from relacionInv in NoUtili
                                              where r.IdContrato == relacionInv.FolioContratoArrto
                                              select relacionInv);
                        if (contXIDEncontrato.Count() > 0)
                        {
                            Console.WriteLine("Datos encontrados para verificar");
                            foreach (var rIDE in contXIDEncontrato)
                            {
                                DateTime fechaID = rIDE.FechaFinOcupacion != null ? (DateTime)rIDE.FechaFinOcupacion :
                                    new DateTime(1990, 1, 1);
                                Console.WriteLine("-FolioContrato " + rIDE.FolioContratoArrto + "-" + fechaID + "-" +
                                    rIDE.IdInmuebleArrendamiento + "-" + rIDE.Fk_IdInstitucion + "-" + rIDE.NombreVialidad + "-" + rIDE.NumExterior);
                            }
                        }
                        else
                        {
                            Console.WriteLine("- Sin datos");
                        }
                        */

                        // Vamos a buscar el inmueble por calle y numero exterior
                        // si funciona con el IdInmueble buscaremos el contrato para tratar de cuadrar información
                        var busqInm = (from inmueble in inmuebles
                                                 where vigente.Calle.Replace("á", "a")
                                                                    .Replace("é", "e")
                                                                    .Replace("í", "i")
                                                                    .Replace("ó", "o")
                                                                    .Replace("ú", "u")
                                                                    .Replace("Á", "A")
                                                                    .Replace("É", "E")
                                                                    .Replace("Í", "I")
                                                                    .Replace("Ó", "O")
                                                                    .Replace("Ú", "U") == inmueble.NombreVialidad.Replace("á", "a")
                                                                    .Replace("é", "e")
                                                                    .Replace("í", "i")
                                                                    .Replace("ó", "o")
                                                                    .Replace("ú", "u")
                                                                    .Replace("Á", "A")
                                                                    .Replace("É", "E")
                                                                    .Replace("Í", "I")
                                                                    .Replace("Ó", "O")
                                                                    .Replace("Ú", "U") & 
                                                    vigente.NumExterior == inmueble.NumExterior
                                                 select inmueble);
                        if (busqInm.Count() > 0)
                        {
                            // Estimo que se puede corregir.
                            
                            Console.WriteLine("Inmueble Encontrado");
                            foreach (var inmueb in busqInm)
                            {
                                Console.WriteLine("-Inmueble " + inmueb.IdInmuebleArrendamiento + "-" + inmueb.CodigoPostal);
                            }
                            
                        }
                        else
                        {
                            Console.WriteLine("- Sin datos " + (++ContratosSinInmueble));
                        }

                    }
                    // Console.ReadKey();
                } 
            }
            Console.WriteLine("Contratos sin inmueble total " + ContratosSinInmueble);
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
