using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using SharpCompress;
namespace Bosch_ImportData
{
    public class TreeViewBosch
    {
        public TreeView BoschTree { get; set; }
        public TreeViewBosch() { }
        public TreeViewBosch(TreeView _treeview)
        {
            BoschTree = _treeview;
        }
        public void PopulateTreeView(Norma norma)
        {
            TreeNode ParentNode = BoschTree.Nodes.Add("$");
            Parametros.DicionarioNodes.Clear();
            Parametros.DicionarioNodes.Add("$", ParentNode);
            

            if (norma.Produtos.Any(x => x.isMissing == true))     
            {
                TreeNode ErrorNode = BoschTree.Nodes.Add("@");  
                Parametros.DicionarioNodesMissing.Clear();
                Parametros.DicionarioNodesMissing.Add("@", ErrorNode);
            }
            foreach (Produto prod in norma.Produtos)
            {
                if(prod.isMissing)
                    NodeCreate(prod, Parametros.DicionarioNodesMissing);
                else
                    NodeCreate(prod, Parametros.DicionarioNodes);
                //AddPathToTreeView(prod, _treeView);
            }
        }
        public void NodeCreate(Produto prod, Dictionary<string, TreeNode> Dicionario )
        {
            string[] parts = prod.FileNameSimplificado.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            Dicionario.TryGetValue(parts[0], out TreeNode nodePai);
            TreeNode nodeParent = nodePai;
                

            foreach (string part in parts)
            {
                if (Dicionario.TryGetValue(part, out TreeNode node))
                {
                    nodeParent = node;
                }
                else
                {
                    TreeNode CurrentNode = nodeParent.Nodes.Add(part);
                    CurrentNode.ForeColor = prod.isMissing ? Color.Red : Color.Black;
                    CurrentNode.Tag = prod;
                    prod.node = CurrentNode;
                    Dicionario.Add(part, CurrentNode);
                    nodeParent = CurrentNode;
                }
              
            }
        }
        public void NodeCreate(string caminho)
        {
            string[] parts = caminho.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            TreeNode nodeParent = BoschTree.TopNode;

            foreach (string part in parts)
            {
                if (Parametros.DicionarioNodes.TryGetValue(part, out TreeNode node))
                {
                    nodeParent = node;
                }
                else
                {
                    TreeNode CurrentNode = nodeParent.Nodes.Add(part);
                    Parametros.DicionarioNodes.Add(part, CurrentNode);
                    nodeParent = CurrentNode;
                }
            }
        }
        private bool NodeExists(TreeNodeCollection nodes, string text)
        {
            return nodes.Cast<TreeNode>().Any(node => node.Text == text);
        }
        public TreeNode FindNode(TreeNodeCollection nodes, string text)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Text == text)
                {
                    return node;
                }
            }
            return null;
        }
        public TreeNode FindAllNode(TreeView treeView, string searchValue)
        {
            // Cria uma pilha para armazenar os nós
            Stack<TreeNode> stack = new Stack<TreeNode>();

            // Adiciona todos os nós raiz do TreeView na pilha
            foreach (TreeNode node in treeView.Nodes)
            {
                stack.Push(node);
            }

            // Processa cada nó na pilha
            while (stack.Count > 0)
            {
                TreeNode currentNode = stack.Pop();

                // Verifica se o nó atual corresponde ao valor procurado
                if (currentNode.Text == searchValue)
                {
                    return currentNode;
                }

                // Adiciona todos os nós filhos do nó atual na pilha
                foreach (TreeNode childNode in currentNode.Nodes)
                {
                    stack.Push(childNode);
                }
            }

            // Retorna null se o nó não foi encontrado
            return null;
        }
    }
}
