using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using SharpCompress;
using System.IO;
using Autodesk.Connectivity.WebServices;
namespace Bosch_ImportData
{
    public static class TreeViewHelper
    {

        public static void PopulateTreeView(TreeView treeview, Norma norma)
        {
            TreeNode ParentNode = treeview.Nodes.Add("$");
            Parametros.DicionarioNodes.Clear();
            Parametros.DicionarioNodes.Add("$", ParentNode);


            if (norma.Produtos.Any(x => x.isMissing == true))
            {
                TreeNode ErrorNode = treeview.Nodes.Add("@");
                Parametros.DicionarioNodesMissing.Clear();
                Parametros.DicionarioNodesMissing.Add("@", ErrorNode);
            }
            foreach (Produto2 prod in norma.Produtos)
            {
                if (prod.isMissing)
                    NodeCreate(prod, Parametros.DicionarioNodesMissing);
                else
                    NodeCreate(prod, Parametros.DicionarioNodes);
                //AddPathToTreeView(prod, _treeView);
            }
        }
        public static void NodeCreate(Produto2 prod, Dictionary<string, TreeNode> Dicionario)
        {
            string nome = prod.FileNameSimplificado.Replace('/', '\\');
            string[] parts = nome.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
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
        public static void NodeCreate(TreeView treeView, string caminho)
        {
            string[] parts = caminho.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            TreeNode nodeParent = treeView.TopNode;



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

        public static void CreateTreeView(TreeView treeView, List<Produto2> Produtos)
        {
            treeView.Nodes.Clear();
            foreach (Produto2 produto in Produtos)
            {
                CreateNodeByPath(treeView, produto);
            }
            treeView.SelectedNode = treeView.TopNode;
        }
        public static void CreateNodeByPath(TreeView treeView, Produto2 prod)
        {
            string FullPath = prod.FileNameSimplificado.Replace('/', '\\');
            string[] parts = FullPath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

            TreeNodeCollection Nodes = treeView.Nodes;
    
            foreach (string part in parts)
            {
                TreeNode NodeCurrent = FindNode(Nodes, part);

                if (NodeCurrent == null)
                {
                    NodeCurrent = new TreeNode(part);
                    NodeCurrent.ForeColor = prod.isMissing ? Color.Red : Color.Black;

                    if (part == parts.Last())
                    {
                        NodeCurrent.Tag = prod;
                        prod.node = NodeCurrent;
                    }
                    else
                    {
                        NodeCurrent.Tag = null;
                    }
                    Nodes.Add(NodeCurrent);
                }
                Nodes = NodeCurrent.Nodes;
            }
        }
        public static void ApagarNodeByPath(TreeView treeView, Produto2 produto)
        {
            TreeNode node = produto.node;
            TreeNode ParentNode = node.Parent;

            treeView.Nodes.Remove(node);

            if (ParentNode == null) return;

            try
            {
                while (ParentNode.Nodes.Count <= 0)
                {
                    if (ParentNode == treeView.TopNode)
                        break;

                    TreeNode nodeCurrent = ParentNode;
                    ParentNode = nodeCurrent.Parent;

                    treeView.Nodes.Remove(nodeCurrent);

                }
            }
            catch (Exception e1)
            {

                Log.gravarLog(e1.Message);
            }
           
        }
        private static bool NodeExists(TreeNodeCollection nodes, string text)
        {
            return nodes.Cast<TreeNode>().Any(node => node.Text == text);
        }
        public static TreeNode FindNode(TreeNodeCollection nodes, string text)
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
        public static TreeNode FindAllNode(TreeView treeView, string searchValue)
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

        private static void AtualizarNodes(List<Produto2> produtos)
        {
            if (produtos.Count == 0) { return; }

            foreach (Produto2 produto in produtos)
            {
                if (produto.node == null) continue;
                produto.node.Text = Path.GetFileName(produto.NewFileName);
            }
        }
        private static void ApagarNodes(TreeView treeView, List<Produto2> produtos)
        {
            if (produtos.Count == 0) return;

            foreach (Produto2 produto in produtos)
            {
                if (produto.node == null) continue;

                try { treeView.Nodes.Remove(produto.node); }
                catch (Exception e1) { Log.gravarLog($"Erro ao remover TreeView Node from: {produto.NewFileName}{System.Environment.NewLine}{e1.Message}"); }
            }
        }
        private static void CriarNode(List<Produto2> produtos)
        {
            if (produtos.Count > 0) return;


        }

       
    }
}
