using System;
using System.Linq;
using System.Threading;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace ComparaciónNoUtili
{
    public class Program
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
            int NoUtiliRegsModified = 0;
            int ContratosSinInmueble = 0;
            //int registrosIrreparables = 0;
            ArrendamientoInmuebleEntities ctx = new ArrendamientoInmuebleEntities();
            var Contratos = ctx.ContratoArrto;
            var Vigentes = ctx.ContratosVigentes;
            var NoUtili = ctx.NoUtiliAuditoria2019T;
            var inmuebles = ctx.InmuebleArrendamiento;
            string SQLqqsaved = @"D:\temp\SQLQQSAVED.txt";
            string LOGqqsaved = @"D:\temp\LOGqqsaved.txt";

            if (File.Exists(SQLqqsaved)) File.Delete(SQLqqsaved);
            StreamWriter SqlFile = new StreamWriter(SQLqqsaved);

            if (File.Exists(LOGqqsaved)) File.Delete(LOGqqsaved);
            StreamWriter LOGFile = new StreamWriter(LOGqqsaved);

            // DbContextTransaction trans = ctx.Database.BeginTransaction();

            // ctx.Database.Log = LOGFile.Write; // produce demasiado texto

            ctx.NoUtiliAuditoria2019P();
            LOGFile.WriteLine("Regeneramos la tabla NoUtili");

            LOGFile.WriteLine("\nCS " + ConfigurationManager.ConnectionStrings["ArrendamientoInmuebleEntities"]);
            
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
                                      FechaFinVigente = vigente.FechaContratoHasta,
                                      IdNoUtili = relacionInv.Identificador
                                  });
                    switch ((int)vigente.FolioContrato)
                    {
                        case 44360:
                        case 42058:
                        case 44421:
                        case 40014:
                        case 40015:
                            LOGFile.WriteLine("Contrato Vigente Corregido " + vigente.FolioContrato);
                            break;
                        default:
                            break;
                    }
                    int numeroRegistros = result.Count();
                    if (numeroRegistros > 0)
                    {
#if MOSTRARBUENOS
                        LOGFile.WriteLine("C /" + vigente.FolioContrato + "/ " + numeroRegistros);
                        foreach (var r in result)
                        {
                            DateTime fecha = r.FechaFin != null ? (DateTime)r.FechaFin :
                            new DateTime(1990, 1, 1);
                            DateTime fechaVigente = r.FechaFinVigente != null ? (DateTime)r.FechaFinVigente :
                            new DateTime(1990, 1, 1);
                            LOGFile.WriteLine("-" + r.FolioContrato + "-" + r.IdContrato + "-" +
                                r.IdInmueble + "-" +
                                r.Inst + "-" +
                                fecha + "-" + fechaVigente);
                        }
#endif
                    }
                    else
                    {
                        // Aqui tenemos un contrato vigente que no está relacionado con un contrato en la relación inversa 
                        // porque el folio del vigente no esta en la tabla NOUTILI, se verifico que los contratos vigentes todos tienen folio Contrato
                        // y que la tabla de inmuebles todos tiene IdInmuebleArrendamiento
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
                        LOGFile.WriteLine("\nI /" + vigente.FolioContrato + "/ " + numeroRegistros + "-" + vigente.Calle + "-" + vigente.NumExterior + " " +
                            vigente.CodigoPostal);
                        // Para estos contratos vigentes hemos encontrado un cntrato que satisface
                        // que los folios de contrato son iguales
                        foreach (var r in incompleto)
                        {
                            DateTime fecha = r.FechaFin != null ? (DateTime)r.FechaFin :
                            new DateTime(1990, 1, 1);
                            DateTime fechaVigente = r.FechaFinVigente != null ? (DateTime)r.FechaFinVigente :
                            new DateTime(1990, 1, 1);
                            LOGFile.WriteLine("-" + r.FolioContrato + "-" + r.IdContrato + "-" +
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
                                              vigente.NumExterior == inmueble.NumExterior &
                                              vigente.CodigoPostal == inmueble.CodigoPostal
                                           select inmueble);
                            switch(busqInm.Count())
                            {
                                case 0:
                                    LOGFile.WriteLine("No se encontró in inmueble para el contrato vigente " + vigente.ID);
                                    break;
                                case 1:
                                    LOGFile.WriteLine("Candidato");
                                    foreach (var inmueb in busqInm)
                                    {
                                        var regNoUtil = (from relinv in NoUtili
                                                         where vigente.Calle == relinv.NombreVialidad &
                                                               vigente.NumExterior == relinv.NumExterior &
                                                               vigente.CodigoPostal == relinv.CodigoPostal.ToString()
                                                         select new
                                                         {
                                                             FolioContrato = vigente.FolioContrato,
                                                             IDNoUtili = relinv.Identificador
                                                         });
                                        LOGFile.WriteLine("-Inmueble " + inmueb.IdInmuebleArrendamiento + "-" +
                                            inmueb.NombreVialidad + "-" +
                                            inmueb.NumExterior + "-" +
                                            inmueb.CodigoPostal + "- regs " + regNoUtil.Count());
                                        switch (regNoUtil.Count())
                                        {
                                            case 0:
                                                LOGFile.WriteLine("No se encontro un registro NoUtili para este contrato");
                                                break;
                                            case 1:
                                                foreach (var rn in regNoUtil)
                                                {
                                                    LOGFile.WriteLine(" NoUtli record " + rn.IDNoUtili);
                                                    SqlFile.WriteLine("Update NoUtiliAuditoria2019T set FolioContratoArrto = " + r.FolioContrato +
                                                    ", IdInmuebleArrendamiento = " + inmueb.IdInmuebleArrendamiento +
                                                    " where Identificador = " + rn.IDNoUtili);
                                                }
                                                break;
                                            default:
                                                LOGFile.WriteLine("Encontramos mas de un registro el NoUtili **** O J O");
                                                foreach (var rn in regNoUtil)
                                                {
                                                    LOGFile.WriteLine(" NoUtli record " + rn.IDNoUtili);
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                default:
                                    LOGFile.WriteLine("Encontramos mas de un candidato");
                                    foreach (var inmueb in busqInm)
                                    {
                                        LOGFile.WriteLine("-Inmueble " + inmueb.IdInmuebleArrendamiento + "-" + inmueb.NombreVialidad + "-" +
                                            inmueb.NumExterior + "-" + inmueb.CodigoPostal);
                                    }
                                    break;
                            }
                        }
                    }
                }
                
                //trans.Commit();
            }
            catch (Exception ex)
            {
                //trans.Rollback();
                LOGFile.WriteLine("Error al actualizar NoUtili " + ex.ToString());
            }
            LOGFile.WriteLine("Registros NoUtili modificados " + NoUtiliRegsModified);
            LOGFile.WriteLine("Contratos sin Inmueble " + ContratosSinInmueble);
            SqlFile.Close();
            LOGFile.Close(); 
        }

        static public string GetPais(int FkIdPais)
        {
            DB_CAT_NuevaEntities ctx = new DB_CAT_NuevaEntities();
            var Pais = (from pay in ctx.Cat_Pais where FkIdPais == pay.IdPais select pay);
            foreach (var p in Pais) return p.DescripcionPais;
            return null;
        }

        static public string CleanExcelString(string input)
        {
            char[] QuitaChars = { '"', ' ', '\\' };
            input = input.Trim(QuitaChars);
            int pos;
            while ((pos = input.IndexOf(',')) >= 0)
            {
                input = input.Remove(pos, 1);
            }
            while ((pos = input.IndexOf('\\')) >= 0)
            {
                input = input.Remove(pos, 1);
            }
            while ((pos = input.IndexOf('$')) >= 0)
            {
                input = input.Remove(pos, 1);
            }
            while ((pos = input.IndexOf(' ')) >= 0)
            {
                input = input.Remove(pos, 1);
            }
            while ((pos = input.IndexOf('"')) >= 0)
            {
                input = input.Remove(pos, 1);
            }
            return input;
        }

        static public Decimal DineroADecimal(int index, string monto)
        {
            if (IsExcelNull(monto))
            {
                return (Decimal)0.0;
            }
            else
            {
                Console.WriteLine("-" + index + "-" + monto);
                monto = CleanExcelString(monto);
                Console.WriteLine("--" + monto);
                return Convert.ToDecimal(monto);
            }
        }

        static public Decimal ExcelNumeroADecimal(int index, string monto)
        {
            if (IsExcelNull(monto))
            {
                return (Decimal)0.0;
            }
            else
            {
                Console.WriteLine("-" + index + "-" + monto);
                monto = CleanExcelString(monto);
                Console.WriteLine("--" + monto);
                return Convert.ToDecimal(monto);
            }
        }

        static public Double ExcelNumeroADouble(int index, string monto)
        {
            if (IsExcelNull(monto))
            {
                return (Double)0.0;
            }
            else
            {
                Console.WriteLine("-" + index + "-" + monto);
                monto = CleanExcelString(monto);
                Console.WriteLine("--" + monto);
                return Convert.ToDouble(monto);
            }
        }


        static public bool IsExcelNull(string valor)
        {
            if (String.IsNullOrEmpty(valor) || valor.Equals("NULL") || String.IsNullOrWhiteSpace(valor)) return true; else return false;
        }

        static void BusquedaVigentesEnReporteTotal()
        {
            ArrendamientoInmuebleEntities ctx = new ArrendamientoInmuebleEntities();
            var Contratos = ctx.ContratoArrto;
            var Vigentes = ctx.ContratosVigentes;
            var Inmuebles = ctx.InmuebleArrendamiento;
            var ReporteTotal = ctx.ReporteTotal;
            string SQLqqsaved = @"D:\temp\SQLQQSAVED.txt";
            string LOGqqsaved = @"D:\temp\LOGqqsaved.txt";

            if (File.Exists(SQLqqsaved)) File.Delete(SQLqqsaved);
            StreamWriter SqlFile = new StreamWriter(SQLqqsaved);

            if (File.Exists(LOGqqsaved)) File.Delete(LOGqqsaved);
            StreamWriter LOGFile = new StreamWriter(LOGqqsaved);

            LOGFile.WriteLine("\nCS " + ConfigurationManager.ConnectionStrings["ArrendamientoInmuebleEntities"]);

            int RegVigente = 0;
            bool RegistrosIguales = true;
            foreach(var vigente in Vigentes)
            {
                LOGFile.Write("{000000}-",++RegVigente);
                var totalEncontrados = (from total in ReporteTotal
                                        where total.FolioContrato == vigente.FolioContrato
                                        select total);
                foreach(var total in totalEncontrados)
                {
                    if (vigente.FolioContrato == total.FolioContrato) LOGFile.Write("1"); else LOGFile.Write("0");
                    if (vigente.Propietario.Equals(total.Propietario)) LOGFile.Write("1"); else LOGFile.Write("0");
                    if (vigente.Responsable.Equals(total.Responsable)) LOGFile.Write("1"); else LOGFile.Write("0");
                    if (vigente.Promovente.Equals(total.Promovente)) LOGFile.Write("1"); else LOGFile.Write("0");
                    string pais = GetPais(total.FkIdPais).ToUpper();
                    if (vigente.Pais.Equals(pais)) LOGFile.Write("1"); else LOGFile.Write("0"); // no letra y numero
                    //if (vigente.Estado + "-" + total.Estado) LOGFile.Write("1"); else LOGFile.Write("0"); // total vacio
                    if (!IsExcelNull(total.Estado)) LOGFile.Write("Edo no vacio-");
                    //if (vigente.Municipio + "-" + total.Municipio) LOGFile.Write("1"); else LOGFile.Write("0"); // total vacio
                    if (!IsExcelNull(total.Municipio)) LOGFile.Write("Municipio no vacio");
                    if (vigente.Colonia.Equals(total.Colonia)) LOGFile.Write("1"); else LOGFile.Write("0");
                    if (vigente.Calle.Equals(total.Calle)) LOGFile.Write("1"); else LOGFile.Write("0");
                    if (vigente.CodigoPostal.Equals(total.CodigoPostal)) LOGFile.Write("1"); else LOGFile.Write("0");
                    if ((IsExcelNull(vigente.NumInterior) && IsExcelNull(total.NumInterior)) || vigente.NumInterior.Equals(total.NumInterior)) LOGFile.Write("1"); else LOGFile.Write("0"); // sin info los dos
                    if (vigente.NumExterior.Equals(total.NumExterior)) LOGFile.Write("1"); else LOGFile.Write("0");
                    if ((IsExcelNull(vigente.Ciudad) && IsExcelNull(total.Ciudad)) || vigente.Ciudad.Equals(total.Ciudad)) LOGFile.Write("1"); else LOGFile.Write("0"); // nulo blanco
                    if (vigente.OtroUsoInmueble.Equals(total.OtroUsoInmueble)) LOGFile.Write("1"); else LOGFile.Write("0");
                    if (vigente.TipoContrato.Equals(total.TipoContrato)) LOGFile.Write("1"); else LOGFile.Write("0");
                    if ((IsExcelNull(vigente.TipoOcupacion) && IsExcelNull(total.TipoOcupacion)) || vigente.TipoOcupacion.Equals(total.TipoOcupacion)) LOGFile.Write("1"); else LOGFile.Write("0"); // vacio NULL
                    if (vigente.DescripcionTipoArrendamiento.Equals(total.DescripcionTipoArrendamiento)) LOGFile.Write("1"); else LOGFile.Write("0");
                    //if (vigente.TipoInmueble + "-" + total.TipoInmueble) LOGFile.Write("1"); else LOGFile.Write("0"); // total sin info
                    if (!IsExcelNull(total.TipoInmueble)) LOGFile.Write("TipoInmueble no vacio");
                    //if (vigente.TipoUsoInmueble + "-" + total.TipoUsoInmueble) LOGFile.Write("1"); else LOGFile.Write("0"); // total sin info
                    if (!IsExcelNull(total.TipoUsoInmueble)) LOGFile.Write("TipoUsoInmueble no vacio");
                    if (vigente.AreaOcupadaM2 == total.AreaOcupadaM2) LOGFile.Write("1"); else LOGFile.Write("0");
                    if ((decimal)vigente.MontoPagoMensual == total.MontoPagoMensual) LOGFile.Write("1"); else LOGFile.Write("0");
                    if ((decimal)vigente.CuotaMantenimiento == total.CuotaMantenimiento) LOGFile.Write("1"); else LOGFile.Write("0");
                    if ((decimal)vigente.MontoPagoPorCajonesEstacionamiento == total.MontoPagoPorCajonesEstacionamiento) LOGFile.Write("1"); else LOGFile.Write("0");
                    if ((decimal)vigente.MontoDictaminado == total.MontoDictaminado) LOGFile.Write("1"); else LOGFile.Write("0");
                    if ((decimal)vigente.RentaUnitariaMensual == total.RentaUnitariaMensual) LOGFile.Write("1"); else LOGFile.Write("0");
                    if ((decimal)vigente.MontoAnterior == total.MontoAnterior) LOGFile.Write("1"); else LOGFile.Write("0");
                    if (vigente.SMOI == total.SMOI) LOGFile.Write("1"); else LOGFile.Write("0");
                    if (vigente.TablaSmoi.Equals(total.TablaSMOI)) LOGFile.Write("1"); else LOGFile.Write("0");
                    if ((DateTime)vigente.Fecha == Convert.ToDateTime(total.Fecha)) LOGFile.Write("1"); else LOGFile.Write("0");
                    if ((DateTime)vigente.FechaContratoDesde == Convert.ToDateTime(total.FechaContratoDesde)) LOGFile.Write("1"); else LOGFile.Write("0");
                    if ((DateTime)vigente.FechaContratoHasta == Convert.ToDateTime(total.FechaCntratoHasta)) LOGFile.Write("1"); else LOGFile.Write("0");
                    if ((DateTime)vigente.FechaDictamen == Convert.ToDateTime(total.FechaDictamen)) LOGFile.Write("1"); else LOGFile.Write("0");
                    if (vigente.DescripcionTipoContratacion.Equals(total.DescripcionTipoContratacion)) LOGFile.Write("1"); else LOGFile.Write("0");
                    if (vigente.ResultadoOpinion.Equals(total.ResultadosOpinion)) LOGFile.Write("1"); else LOGFile.Write("0");
                    if (vigente.RIUF.Equals(total.RIUF)) LOGFile.Write("1"); else LOGFile.Write("0");
                    LOGFile.WriteLine();
                }
            }
            
            LOGFile.WriteLine("Registros NoUtili modificados ");
            LOGFile.WriteLine("Contratos sin Inmueble ");
            SqlFile.Close();
            LOGFile.Close();
        }

        static void Main(string[] args)
        {
            //comparacionNoUtili();
            //BusquedaVigentes();
           
            BusquedaVigentesEnReporteTotal();

           
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
                           LOGFile.WriteLine("Datos encontrados para verificar");
                           foreach (var rIDE in contXIDEncontrato)
                           {
                               DateTime fechaID = rIDE.FechaFinOcupacion != null ? (DateTime)rIDE.FechaFinOcupacion :
                                   new DateTime(1990, 1, 1);
                               LOGFile.WriteLine("-FolioContrato " + rIDE.FolioContratoArrto + "-" + fechaID + "-" +
                                   rIDE.IdInmuebleArrendamiento + "-" + rIDE.Fk_IdInstitucion + "-" + rIDE.NombreVialidad + "-" + rIDE.NumExterior);
                           }
                       }
                       else
                       {
                           LOGFile.WriteLine("- Sin datos");
                       }
                       */

/*
            foreach(NoUtiliAuditoria2019T renglonTest in tablaTest)
            {
                DateTime fecha = renglonTest.FechaFinOcupacion != null ? (DateTime) renglonTest.FechaFinOcupacion :
                    new DateTime(1990,1,1);
                LOGFile.WriteLine("Test " + renglonTest.FolioContratoArrto + " " +
                                            renglonTest.IdInmuebleArrendamiento + " " +
                                            renglonTest.Fk_IdInstitucion + " " +
                                            fecha.ToString("yyyy MMMM dd"));
                
            }
            */

/*
        
 */

/*
 // Aqui tenemos un contrato vigente que no está relacionado con un contrato en la relación inversa 
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
                    LOGFile.WriteLine("\nI /" + vigente.FolioContrato + "/ " + numeroRegistros + "-" + vigente.Calle + "-" + vigente.NumExterior + " " +
                        vigente.CodigoPostal);
                    // Para estos contratos vigentes hemos encontrado un cntrato que satisface
                    // que los folios de contrato son iguales
                    if (incompleto.Count() == 1)
                    {
                        foreach (var r in incompleto)
                        {
                            NoUtiliRegsModified++;
                            LOGFile.WriteLine("-" + r.FolioContrato + " para insertar en NoUtili " + r.IDNoUtili);
                            SqlFile.WriteLine("Update NoUtiliAuditoria2019T set FolioContratoArrto = " + r.FolioContrato + " where Identificador = " + r.IDNoUtili); 
                        }
                    }
                    else
                    {
                        registrosIrreparables++;
                        LOGFile.WriteLine("0 o varios registros noutili que cumplen la condición");
                        foreach (var r in incompleto)
                        {
                            LOGFile.WriteLine("-" + r.FolioContrato + " para insertar en NoUtili " + r.IDNoUtili);
                        }
                    }
 */
