//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.18444
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;

namespace MapReader
{
	public class Tile
	{
        public bool collides;
		public Rect area;
		int gid;

		public Tile (Rect area, int gid)
		{
			this.area = area;
			this.gid = gid;
		}
	}
}

