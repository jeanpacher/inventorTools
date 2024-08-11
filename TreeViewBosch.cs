using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
namespace Bosch_ImportData
{
    public class TreeViewBosch
    {
        public TreeViewBosch() { }
        public void TreeCreate (TreeView _treeView, Norma norma)
        {
            foreach (Produto prod in norma.Produtos)
            {
                AddPathToTreeView(prod, _treeView);
            }
        }
        private void AddPathToTreeView(Produto prod , TreeView _treeView)
        {
            // Split the path into parts
            string[] parts = prod.FileNameSimplificado.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            TreeNode currentNode = null;
            foreach (string part in parts)
            {
                if (currentNode == null)
                {
                    // Check if the root node already exists
                    if (!NodeExists(_treeView.Nodes, part))
                    {
                       _treeView.Nodes.Add(prod.node = new TreeNode
                        {
                            Text = part,
                            Tag = prod
                        });
                    }
                    currentNode = FindNode(_treeView.Nodes, part);
                }
                else
                {
                    // Check if the child node already exists
                    if (!NodeExists(currentNode.Nodes, part))
                    {
                        currentNode.Nodes.Add(prod.node = new TreeNode
                        {
                            Text = part,
                            Tag = prod
                        });
                    }
                    currentNode = FindNode(currentNode.Nodes, part);
                }
                if (prod.isMissing)
                    currentNode.ForeColor = Color.Red;
                else
                    currentNode.ForeColor = Color.Black;
            }
        }
        private bool NodeExists(TreeNodeCollection nodes, string text)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Text == text)
                {
                    return true;
                }
            }
            return false;
        }
        private TreeNode FindNode(TreeNodeCollection nodes, string text)
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
    }
}
