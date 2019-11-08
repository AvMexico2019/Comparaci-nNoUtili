using System;
using System.Linq;
using System.Threading;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

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

        static void ModificaNoUtili(StreamWriter file, int FolioContrato, int Identificador)
        {
            
        }

        static void BusquedaVigentes()
        {
            int NoUtiliRegsModified = 0;
            int registrosIrreparables = 0;
            ArrendamientoInmuebleEntities ctx = new ArrendamientoInmuebleEntities();
            var Contratos = ctx.ContratoArrto;
            var Vigentes = ctx.ContratosVigentes;
            var NoUtili = ctx.NoUtiliAuditoria2019T;
            var inmuebles = ctx.InmuebleArrendamiento;
            string SQLqqsaved = @"D:\temp\SQLQQSAVED.txt";

            if (File.Exists(SQLqqsaved)) File.Delete(SQLqqsaved);
            StreamWriter SqlFile = new StreamWriter(SQLqqsaved);

            // DbContextTransaction trans = ctx.Database.BeginTransaction();

            // ctx.Database.Log = Console.Write; // produce demasiado texto

            // ctx.NoUtiliAuditoria2019P();
            // Console.WriteLine("Regeneramos la tabla NoUtili");

            Console.WriteLine("\nCS " + ConfigurationManager.ConnectionStrings["ArrendamientoInmuebleEntities"]);
            
            try
            {
                foreach (var vigente in Vigentes)
                {
                    // localizamos el inmueble del contrato vigente via la relacion inversa y la condición es:
                    // que el IDInmueble de contrao sea igual al de la relación inversa y que
                    // el folio del contrato vigente sea igual al folio del contrato (de la tabla de contratos)
                    // de esta forma estamos probando la relación inversa
                    // result contiene los contratos vigentes que estan en la base cuyo inmueble lo podemos encontrar
                    // aun es necesaro verificar que los datos del inmueble sean corretos
                    var result = (from contrato in Contratos
                                  join relacionInv in NoUtili
                                  on contrato.Fk_IdInmuebleArrendamiento equals relacionInv.IdInmuebleArrendamiento
                                  where vigente.FolioContrato == contrato.FolioContratoArrto
                                  select new
                                  {
                                      FolioContrato = contrato.FolioContratoArrto,
                                      IdContrato = contrato.IdContratoArrto,
                                      IdInmueble = contrato.Fk_IdInmuebleArrendamiento,
                                      Inst = contrato.Fk_IdInstitucion,
                                      FechaFin = contrato.FechaFinOcupacion,
                                      FechaFinVigente = vigente.FechaContratoHasta
                                  });
                    switch ((int)vigente.FolioContrato)
                    {
                        case 44360:
                        case 42058:
                        case 44421:
                        case 40014:
                        case 40015:
                            Console.WriteLine("Contrato Corregido");
                            break;
                        default:
                            break;
                    }
                    int numeroRegistros = result.Count();
                    if (numeroRegistros > 0)
                    {
#if MOSTRARBUENOS
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
#endif
                    }
                    else
                    {   // Aqui tenemos un contrato vigente que no está relacionado con un contrato en la relación inversa 
                        // porque el folio del vigente no esta en la tabla NOUTILI, se verifico los contratos vigentes todos tienen folio Contrato
                        // Vamos a identificar porque la tabla NOUTILI esta incompleta, primero vamos a corregir NOUTILI.
                        // Es necesario identificar el numero minimo de condiciones para corregir el problema.
                        // Lo que debe de ocurrir es que este programa deje de encontrar registros que caigan en esta parte.
                        // vamos a tratar de reconstruir la relación via los datos del inmueble
                        // incompleto contiene los contratos vigentes que estan en la base y no tienen un inmueble localizable 
                        var incompleto = (from relinv in NoUtili
                                          where vigente.Calle == relinv.NombreVialidad &
                                                vigente.NumExterior == relinv.NumExterior &
                                                vigente.CodigoPostal == relinv.CodigoPostal.ToString()
                                          select new
                                          {
                                              FolioContrato = vigente.FolioContrato,
                                              IDNoUtili = relinv.Identificador
                                          });
                        Console.WriteLine("\nI /" + vigente.FolioContrato + "/ " + numeroRegistros + "-" + vigente.Calle + "-" + vigente.NumExterior + " " +
                            vigente.CodigoPostal);
                        // Para estos contratos vigentes hemos encontrado un cntrato que satisface
                        // que los folios de contrato son iguales
                        if (incompleto.Count() == 1)
                        {
                            foreach (var r in incompleto)
                            {
                                NoUtiliRegsModified++;
                                Console.WriteLine("-" + r.FolioContrato + " para insertar en NoUtili " + r.IDNoUtili);
                                SqlFile.WriteLine("Update NoUtiliAuditoria2019T set FolioContratoArrto = " + r.FolioContrato + " where Identificador = " + r.IDNoUtili); 
                            }
                        }
                        else
                        {
                            registrosIrreparables++;
                            Console.WriteLine("0 o varios registros noutili que cumplen la condición");
                            foreach (var r in incompleto)
                            {
                                Console.WriteLine("-" + r.FolioContrato + " para insertar en NoUtili " + r.IDNoUtili);
                            }
                        }
                    }
                }
                
                //trans.Commit();
            }
            catch (Exception ex)
            {
                //trans.Rollback();
                Console.WriteLine("Error al actualizar NoUtili " + ex.ToString());
            }
            SqlFile.Close();
            Console.WriteLine("Registros NoUtili modificados " + NoUtiliRegsModified);
            Console.WriteLine("Registros irreparables" + NoUtiliRegsModified);
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

/*
        // Aqui tenemos un contrato vigente que no está relacionado con un contrato en la relación inversa 
                // porque el folio del vigente no esta en la tabla NOUTILI, se verifico los contratos vigentes todos tienen folio Contrato
                // Vamos a identificar porque la tabla NOUTILI esta incompleta, primero vamos a corregir NOUTILI.
                // Es necesario identificar el numero minimo de condiciones para corregir el problema.
                // Lo que debe de ocurrir es que este programa deje de encontrar registros que caigan en esta parte.
                // vamos a tratar de reconstruir la relación via los datos del inmueble
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
                // Para estos contratos vigentes hemos encontrado un cntrato que satisface
                // que los folios de contrato son iguales
                foreach (var r in incompleto)
                {
                    DateTime fecha = r.FechaFin != null ? (DateTime)r.FechaFin :
                    new DateTime(1990, 1, 1);
                    DateTime fechaVigente = r.FechaFinVigente != null ? (DateTime)r.FechaFinVigente :
                    new DateTime(1990, 1, 1);
                    Console.WriteLine("-" + r.FolioContrato + "-" + r.IdContrato + "-" +
                        r.Inst + "-" +
                        fecha + "-" + fechaVigente);

                    // 
                    // Vamos a buscar el inmueble por calle y numero exterior usando el idinmueble del contrato y comparando la
                    // direccion del inmueble y del contrato vigente
                    // si funciona con el IdInmueble buscaremos el contrato para tratar de cuadrar información
                    // Puede ocurrir que haya correcciones manuales en los nombres de las calles
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
                                                                .Replace("Ú", "U") == 
                                                   inmueble.NombreVialidad.Replace("á", "a")
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
                        // Hemos encontrado inmuebles que coinciden en calle, numro exterior y codigo postal con los datos del contrato
                        // vigente, procedemos a correguir NOUTILI
                        if (busqInm.Count() ==1) // tenemos un candidato
                        {

                        }
                        Console.WriteLine("Inmueble Encontrado, NOUTILI corregido");
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
 */
