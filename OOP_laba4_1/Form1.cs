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

		public Form1()
        {
            InitializeComponent();
			myStorage = new MyStorage(10);
			
		}   

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
			g = CreateGraphics();

		}

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
			Point click;
			click = e.Location;

			myStorage.setCircle(g, click.X, click.Y);

        }
    }



	public class CCircle
	{
		public int x;
		public int y;
		public int r;
		private bool selected; // флаг, выбрал ли объект

		public CCircle(int x, int y)
		{
			this.x = x;
			this.y = y;
			r = 50;
			selected = false;

		}

		// функция рисует круг
		public void showCircle(Graphics g)
		{
			Pen blackPen = new Pen(Color.Black, 3);
			g.DrawEllipse(blackPen, x, y, r+r, r+r);
		}
	}

	class MyStorage
	{
		private int size;//размер массива
		private CCircle[] storage;// хранилище
		private CCircle[] tmp_storage;// временное хранилище

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

		//определяет на какую позицию добавить объект в массив
		public void setCircle(Graphics g, int x, int y)
		{
			int emptyPosition = getEmptyPosition();
			if (emptyPosition == -1) // значит в массиве нет места для создания нового объекта
            {
				setObject(size, new CCircle(x, y));
				storage[size-1].showCircle(g);

			} else
            {
				setObject(emptyPosition, new CCircle(x, y));
				storage[emptyPosition].showCircle(g);
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



		//удаление объекта с указанной позиции
		void deleteObject(int position)
		{

		}

		//функция проходит по всему массиву и вызывает метод showClass у всех объектов
		void callShowMethod(Graphics g)
		{
			for (int i = 0; i < size; i++)
			{
				if (!isEmptyPosition(i))
				{
					storage[i].showCircle(g);
				}
			}
		}
	};





}
