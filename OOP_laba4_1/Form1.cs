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
		Color colorForm = Color.Silver; // цвет фона

		public Form1()
        {
            InitializeComponent();
			myStorage = new MyStorage(10);
			Size = new System.Drawing.Size(600, 800); //размер формы
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
			int choiceObject = myStorage.checkCoord(click.X, click.Y);

			if (Control.ModifierKeys == Keys.Control)
            {
				if (choiceObject != -1) // попадаем в объект
                {
					myStorage.setSelected(choiceObject);
					myStorage.callShowMethod(g);
				}
			} 
			else
            {
				if (choiceObject != -1) // попадаем в объект
				{
					myStorage.unSelectedObject();
					myStorage.setSelected(choiceObject);
					myStorage.callShowMethod(g);
				}
				else
				{
					myStorage.setCObject(g, new CCircle(click.X, click.Y));
				}
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
        }
    }

	public class CObject
    {
		public int x, y;
		public Pen pen; // цвет контура фигуры
		public bool selected; // флаг, выбрал ли объект

		public virtual void showObject(Graphics g)
		{}

		public virtual bool checkCoord(int x, int y)
		{
			return false;
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

		// возвращает флаг выбранности объекта
		public bool isSelected()
		{
			return selected;
		}

	}


	public class CCircle: CObject
	{
		public int r;
		public CCircle(int x, int y)
		{
			r = 50;
			this.x = x - r;
			this.y = y - r;
			selected = true;
		}

		// функция рисует круг
		public override void showObject(Graphics g)
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

		// функция проверяет кликнул ли пользователь внутрь круга, или нет
		// возвращает true - если внутри, false - иначе
		public override bool checkCoord(int x, int y)
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
		private CObject[] storage;// хранилище

		//конструктор
		public MyStorage(int size)
		{			
			this.storage = new CObject[size];
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
			for (int i = 0; i < size; i++)
			{
				if (position == -1 && isEmptyPosition(i))
				{
					position = i;
				}
			}
			return position;
		}

		// функция проверяет кликнул ли пользователь внутрь какого либо объекта, или нет
		// если "внутрь объекта", то возвращается индекс объекта
		// иначе -1
		public int checkCoord(int x, int y)
        {
			int result = -1;
			for (int i = 0; i< size; i++)
            {
				if (!isEmptyPosition(i))
                {
					if (((CObject)storage[i]).checkCoord(x, y))
					{
						result = i;
					}
				}
            }
			return result;
        }

		//определяет на какую позицию добавить объект в массив
		public void setCObject(Graphics g, CObject newObj)
		{
			int emptyPosition = getEmptyPosition();
			if (emptyPosition == -1) // значит в массиве нет места для создания нового объекта
            {
				unSelectedObject();
				setObject(size, newObj);
				callShowMethod(g);

			} else
            {
				unSelectedObject();
				setObject(emptyPosition, newObj);
				callShowMethod(g);
			}
		}

		// добавить объект на указанную позицию
		// если хранилище меньше, чем заданная позиция,
		// то расширяем хранилище
		void setObject(int position, CObject obj)
		{
			if (position < size)
			{
				storage[position] = obj;
			}
			else
			{
				int newSize = position + 1;
				Array.Resize(ref storage, newSize);
				storage[position] = obj;
				this.size = newSize;
			}

		}

		// получить объект на i-той позиции (без удаления)
		Object getObject(int i)
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

		//функция проходит по всему массиву и вызывает метод showObject у всех объектов
		public void callShowMethod(Graphics g)
		{
			for (int i = 0; i < size; i++)
			{
				if (!isEmptyPosition(i))
				{
					storage[i].showObject(g);
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

		// функция делает конкретный объект выбранным
		// принимает позицию объекта который нужно выделить
		public void setSelected(int i)
        {
			storage[i].setSelected();
		}
	};
}
