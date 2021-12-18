using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace OOP_laba4_1
{


	public partial class Form1 : Form
    {
		MyStorage myStorage;
		Graphics g;
		Color colorForm = Color.Gray; // цвет фона

		public Form1()
        {
            InitializeComponent();
			myStorage = new MyStorage(10);
			
		}   

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
			g = CreateGraphics();
			g.Clear(colorForm);
			myStorage.callShowMethod(g);

		}

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
			Point click;
			click = e.Location;

			int choiceCircle = myStorage.checkCoord(click.X, click.Y);

			if (choiceCircle == -1)
            {
				myStorage.setCircle(g, click.X, click.Y);
			} else
            {
				myStorage.unSelectedObject();
				myStorage.setSelected(choiceCircle);
				myStorage.callShowMethod(g);
			}


			

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
			if (e.KeyData == Keys.Delete)
            {
				myStorage.deleteSelectedObject();
				g.Clear(colorForm);
				myStorage.callShowMethod(g);
			}
			if (e.Control)
            {
				// обработчик события ctrl
            }
        }
    }



    public class CCircle
	{
		public int x;
		public int y;
		public int r;
		private Pen pen; // цвет контура круга
		private bool selected; // флаг, выбрал ли объект

		public CCircle(int x, int y)
		{
			r = 50;
			this.x = x - r;
			this.y = y - r;
			selected = true;

		}

		// функция рисует круг
		public void showCircle(Graphics g)
		{
			if (selected)
            {
				pen = new Pen(Color.Red, 3);
			} else
            {
				pen = new Pen(Color.Black, 3);
			}
			
			g.DrawEllipse(pen, x, y, r+r, r+r);
		}

		// помечает объект как не выбранный
		public void unSelected()
        {
			selected = false;
        }

		//делает объект выбранным
		public void setSelected()
        {
			selected = true;
        }

		// возвращает флаг выбранности круга
		public bool isSelected()
        {
			return selected;
        }

		// функция проверяет кликнул ли пользователь внутрь круга, или нет
		// возвращает true - если внутри, false - иначе
		public bool checkCoord(int x, int y)
        {
			int x_center = this.x + r;
			int y_center = this.y + r;

			double distance = Math.Sqrt((x-x_center) * (x - x_center) + (y - y_center) * (y - y_center));

			return distance <= r;
        }
	}

	class MyStorage
	{
		private int size;//размер массива
		private CCircle[] storage;// хранилище

		//конструктор
		public MyStorage(int size)
		{
			this.storage = new CCircle[size];
			for (int i = 0; i < size; i++)
			{
				this.storage[i] = null;
			}
			this.size = size;
		}

		// получить размер хранилища
		int getCount()
		{
			return size;
		}

		// Получить индекс свободной ячейки.
		// Если такая ячейка найдена, то метод возвращает ее позицию.
		// Иначе возвращает -1
		protected int getEmptyPosition()
        {
			int position = -1;
			for (int i=0; i < size; i ++)
            {
				if (position == -1 && isEmptyPosition(i))
                {
					position = i;
                }
            }
			return position;
        }

		// функция проверяет кликнул ли пользователь внутрь какого либо круга, или нет
		// если "внутрь круга", то возвращается индекс круга
		// иначе -1
		public int checkCoord(int x, int y)
        {
			int result = -1;
			for (int i = 0; i< size; i++)
            {
				if (!isEmptyPosition(i))
                {
					if (storage[i].checkCoord(x, y))
                    {
						result = i;
					}
				}
            }
			return result;
        }

		//определяет на какую позицию добавить объект в массив
		public void setCircle(Graphics g, int x, int y)
		{
			int emptyPosition = getEmptyPosition();
			if (emptyPosition == -1) // значит в массиве нет места для создания нового объекта
            {
				unSelectedObject();
				setObject(size, new CCircle(x, y));
				callShowMethod(g);

			} else
            {
				unSelectedObject();
				setObject(emptyPosition, new CCircle(x, y));
				callShowMethod(g);
			}

		}

		// добавить объект на указанную позицию
		// если хранилище меньше, чем заданная позиция,
		// то расширяем хранилище
		void setObject(int position, CCircle circle)
		{
			if (position < size)
			{
				storage[position] = circle;
			}
			else
			{
				int newSize = position + 1;
				Array.Resize(ref storage, newSize);
				storage[position] = circle;
				this.size = newSize;
			}

		}

		// получить объект на i-той позиции (без удаления)
		CCircle getObject(int i)
		{
			return storage[i];
		}

		// проверяет наличие объекта на i-той позици
		bool isEmptyPosition(int i)
		{
			bool result;
			if (storage[i] == null)
			{
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		//удаляет выбранные объекты
		public void deleteSelectedObject()
        {
			for (int i = 0; i < size; i++)
            {
				if (!isEmptyPosition(i))
                {
					if (storage[i].isSelected())
                    {
						storage[i] = null;
					}
				}
				
            }
        }

		//функция проходит по всему массиву и вызывает метод showCircle у всех объектов
		public void callShowMethod(Graphics g)
		{
			for (int i = 0; i < size; i++)
			{
				if (!isEmptyPosition(i))
				{
					storage[i].showCircle(g);
				}
			}
		}

		// функция делает все объекты не выбранными
		public void unSelectedObject()
        {
			for (int i = 0; i < size; i++)
			{
				if (!isEmptyPosition(i))
				{
					storage[i].unSelected();
				}
			}
		}

		// функция делает конткретный объект выбранным
		// принимает позицию объекта который нужно выделить
		public void setSelected(int i)
        {
			storage[i].setSelected();

		}
	};





}
