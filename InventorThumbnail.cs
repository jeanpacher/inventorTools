using Inventor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bosch_ImportData
{
    public static class InventorThumbnail
    {

        public static Image GetThumbnail(Document activeDoc)
        {
            if (activeDoc == null)
            {
                Log.GravarLog("Documento nulo. Erro ao gerar Thumbnail");
                return null;
            }

            // Crie uma imagem do modelo ativo
            return CreateThumbnail1(activeDoc, new Size(200, 200));

            
        }

        private static Image CreateThumbnail1(Document document, Size newSize)
        {
            try
            {
                // Salve uma imagem temporária do modelo
                string tempImagePath = System.IO.Path.GetTempFileName() + ".png";
                document.SaveAs(tempImagePath, true);

                // Carregue a imagem como um objeto Image
                Image originalImage = Image.FromFile(tempImagePath);

                // Redimensione a imagem para o novo tamanho
                Image thumbnail = new Bitmap(newSize.Width, newSize.Height);
                using (Graphics g = Graphics.FromImage(thumbnail))
                {
                    g.DrawImage(originalImage, 0, 0, newSize.Width, newSize.Height);
                }

                // Exclua a imagem temporária
                //File.Delete(tempImagePath);

                return thumbnail;
            }
            catch (Exception ex)
            {
               Log.GravarLog("Erro ao criar o thumbnail: " + ex.Message);
                return null;
            }

        }

        private static Image CreateThumbnail(Document document)
        {
            Image thumbnail;

            try
            {
                // Salve uma imagem temporária do modelo
                string tempImagePath = System.IO.Path.GetTempFileName() + ".png";
                document.SaveAs(tempImagePath, true);

                // Carregue a imagem como um objeto Image
                thumbnail = Image.FromFile(tempImagePath);

                // Exclua a imagem temporária
                //File.Delete(tempImagePath);

                return thumbnail;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao criar o thumbnail: " + ex.Message);
                return null;
            }

        }
    }
}
