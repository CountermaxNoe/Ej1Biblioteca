using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Unidad2.Models;

namespace Unidad2.ViewModels
{
    public enum Ventanas { Agregar,Eliminar,Lista}
    public class LibrosViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Libro> Libros { get; set; } = new();
        //Accione de usuario
        public ICommand AgregarCommand { get; set; }
        public ICommand VerAgregarCommand { get; set; }
        public ICommand VerEliminarCommand {  get; set; }
        public ICommand VerEditarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand GuardarCommand { get; set; }
        //Referenecia Modelo
        public Libro? Libro { get; set; }
        //Muestra errores
        public string Error { get; set; } = "";
        //decide que ventana mostrar
        public Ventanas Ventana { get; set; } = Ventanas.Lista;
        public LibrosViewModel() 
        {
            CancelarCommand= new RelayCommand(Cancelar);
            AgregarCommand = new RelayCommand(Agregar);
            GuardarCommand = new RelayCommand(Guardar);
            VerAgregarCommand = new RelayCommand(VerAgregar);
            VerEliminarCommand = new RelayCommand(VerEliminar);
            VerEditarCommand = new RelayCommand(VerEditar);
            EliminarCommand = new RelayCommand(Eliminar);
            Deserializar();
        }

        private void Eliminar()
        {
            if (Libro != null)
            {
                Libros.Remove(Libro);
                Serializar();
                Ventana = Ventanas.Lista;
                Actualizar(nameof(Ventana));
            }

        }

        private void VerEditar()
        {
            throw new NotImplementedException();
        }

        private void VerEliminar()
        {
            Ventana = Ventanas.Eliminar;
            Actualizar(nameof(Ventana));
        }

        private void VerAgregar()
        {
            Libro = new();//aqui se capturan los datos del libro que queremos añadir
            Ventana = Ventanas.Agregar;
            Actualizar(nameof(Ventana));
        }

        private void Guardar()
        {
            throw new NotImplementedException();
        }

        private void Agregar()
        {
            if (Libro != null)
            {
                Error = "";
                if (string.IsNullOrWhiteSpace(Libro.Titulo))
                {
                    Error = "Escriba el nombre del titulo";
                    Actualizar(nameof(Error));
                    return;
                }
                if (string.IsNullOrWhiteSpace(Libro.Autor))
                {
                    Error = "Escriba el nombre del Autor";
                    Actualizar(nameof(Error));

                    return;
                }
                if (string.IsNullOrWhiteSpace(Libro.Portada)|| Libro.Portada.StartsWith("http")||!Libro.Portada.EndsWith(".jpg"))
                {
                    Error = "Escriba el link de una imagen JPEG";
                    Actualizar(nameof(Error));

                    return;
                }
                Libros.Add(Libro);
                Serializar();

                Ventana = Ventanas.Lista;
                Actualizar(nameof(Ventana));
            }
        }
        void Serializar()
        {
            File.WriteAllText("libros.json", JsonSerializer.Serialize(Libros));
        }
        void Deserializar()
        {
            try
            {
                var datos=JsonSerializer.Deserialize<ObservableCollection<Libro>>(File.ReadAllText("libros.json"));
                if (datos != null)
                {
                    foreach (var libro in datos)
                    {
                        Libros.Add(libro);
                    }
                }
            }
            catch
            {
                
            }
        }

        private void Cancelar()
        {
            Ventana = Ventanas.Lista;
            Actualizar(nameof(Ventana));
        }
        //Encapsulo el propertychanged
        private void Actualizar(string? name)
        {
            PropertyChanged?.Invoke(this, new(name));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
