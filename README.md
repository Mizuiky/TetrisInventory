README

## Supreme Tile Inventory

(Disclaimer: Essa 'documentação' só está em portugues porque é mais simples de explicar. Normalmente, faria em ingles.)

### Ideia básica:

Um inventário que possa conter itens com diversos tamanhos diferentes, e poder organizar entre eles.
No fim, o proprio inventário se torna um mini quebra-cabeça para o jogador, deixando o jogo mais estratégico do que apenas
uma bolsa da hermione.

### Observacoes:
- Dentro da pasta Resources em Assets/Resources/SaveData deixei o json limpo do inventario e da configuracao dos items adicionados
- estava usando o persistent data path do meu pc mas como no de voces sera diferente entao seria so atualizar os jsons criados em parsistentData path + SaveData/ utilizando os jsons do Resources folder

persistent data path aqui no meu : C:\Users\gabri\AppData\LocalLow\DefaultCompany\Inventory\SaveData

---

## Descoberta:
Isso obviamente é mais dificil do que parece, justamente porque os itens precisam manter uma forma de 'encaixar' no inventário,
de tal forma que ele saiba qual peça está em cada lugar, para possibilitar com que as outras também possam ser encaixadas.

A primeira abordagem, foi tentar utilizar collider para as peças, para umas não poderem tocar umas nas outras, mas isso não funcionou
tão bem porque por exemplo usando imagem estilo L o collider iria cobrir a imagem por inteiro que 'e um retangulo e nao somente o formato em L em si, entao descartei esta possibilidade

Uma segunda abordagem, foi em usar matrizes, tanto para o investário quando para cada peça.
Dessa forma, cada peça teria sua propria "configuração" que assume no inventário (eliminando o problema com o transparente).
E o inventário, por sua vez, consegue sempre determinar qual peça consegue encaixar em cada lugar.

Computacionalmente isso não é performático (fazer vários loops em matriz), mas como o tamanho geral do inventário não é grande apesar de ser configuravel
não creio que será um problema.
Unico ponto negativo (até o momento) é que o código acaba ficando bem mais complexo.

## Adicionando primeira peca ao inventario

A ideia foi para cada novo item adicionado, criar uma configuracao formada por 0 e 1 onde 1 representa na matriz de configuracao o local onde existe um quadrado 64x64 que compoe a peca
entao seguindo o exemplo do L ficaria:

|   A   |   B   |   C   |
|-------|-------|-------|      
| 0,0   | 0,1   | 0,2   |       
| 1,0   | 1,1   | 1,2   |       


# = L    

|   A   |   B   |   C   |
|-------|-------|-------|      
|   1   |   0   |   0   |       
|   1   |   1   |   1   |  

Quando adicionamos uma nova peca ao inventario:

1) Passamos por cada um dos slots dele checando se nao ha pecas anexadas a ele
2) Se nao houver instanciamos no inventario a peca correspondente
3) Extraimos sua configuracao
4) Para cada item da matriz de configuracao , tenho como parametros a linha e coluna do slot atual que esta sendo verificado
5) Armazeno o indice atual do slot
6) Checo se existe o numero 1 e o slot nao tem items anexados
7) Se isso for satisfeito eu armazeno os dados do indice em que posso encaixar a peca
8) Repito isso ate que tenha visto cada item da matrz de configuracao do item
9) Caso nao for satisfeito, esse slot atual nao é um canditato a anexar o meu item, entao o for que passa pelo inventario continua para o proximo voltando para o numero 1)

10) Quando todas as condicoes forem satisfeitas e minha configuracao for possivel encaixar no meu slot atual
11) Seto o parent do meu item para o transform do meu inventario, e atualizo a posicao da minha peca seguindo uma constante utilizando como base a altura, largura da image e altura e largura do meu slot

12) Para cada index de slot em que meu item é encaixado eu atualizo o meu inventoryItemData que contem um lista de slots em que o item é encaixado
13) O Slot referente ao inventario é atualizado com o has item = true indicando que 'a um novo item anexado a ele
14) Passo o id do item para o slot saber depois qual o item tem dentro dele
15) O item 'e adicionado a uma lista de items do meu inventario, 
16) Os dados do inventario e do item sao salvos.

---

## Tool para Adicionar novos items

Campos:

- Nome do item
- ID
- Type(None, Material, Weapon, Ammo, Consumable)
- Sprite(sprite referente ao item)
- Descricao do item
- Imagem do inventario(selecionar uma das pecas dentro da pasta Assets/UI/InventoryItems
- ImageConfig(configuracao em formato de matriz da peca do inventario)
   - Linhas
   - Colunas

Ao preencher todos os campos aparecerá um botao para criar o item

- O item builder cria prefabs com as configuracoes setadas para um item normal e um item de inventario
armazena esses prefabs na Pasta Resources/Prefabs/InventoryItems  e  Resources/Prefabs/Items

- Os dados dos items criados sao salvos em json dentro do caminho C:\Users\[Seu usuario]\AppData\LocalLow\DefaultCompany\Inventory\GameData
- O arquivo referente aos items se chama ItemData, e para os de inventario, InventoryData

- Quando o jogo é iniciado, é feito o load dos dados desses arquivos, alem disso é feito o load de cada um dos prefabs de items e inventory items
o dados correspondendte de cada um é encaixado e o item manager adiciona esses itens e dados em listas para que sejam de facil acesso.

<img src="assets/ReadMe/tool.png" alt="item tool" width="350"/>

---

## Tool para criar o inventario

Aqui temos um Inventory Builder, que a partir de campos cria um novo inventario

Campos:
- Prefab do slot que compoem o inventario
- Parent para encaixar o inventario na UI
- O parent para poder ser um container para os itens do inventario
- Sprite com o slot normal
- Sprite com slot iluminado
- Posicao x inicial 
- Posicao y inicial
- Quantidade de linhas
- Quantidade de colunas

Para cada indice da matriz linha e coluna é instanciado um novo slot na posicao x,y, alem disso o slot é inicializado com seu indice posicao e dados salvos
Apos o termino da matriz o inventario é inicializado com seus slots, parent, e setado no UIController

---

## Rotacionar items

- Espace key rotaciona em 90 graus a peca para a esquerda

---

## Como spawnar o primeiro item

- Na hierarchy no prefab Spawner, digitar um numero de 0 a 17 que sao a quantidade de items cadastrados  no campo Item Id To Spawn
- Na game Scene clicar no botao rosa Spawn item.
- Deixei o inventario comecando na posicao 0,0 e com 5 linhas e 6 colunas.

---

# Proximos passos do projeto

## Movimentacao das pecas no inventario

Implementar a movimentacao da peca, sendo que para movimentar, w = width e h = height

Precisamos usar a constante:

### Vector3(wImg/2 - wSlot/2, -hImg/2 + hSlot/2) que encaixa a peca.

e movimentar usando ela usando:

matriz [m,n]
onde m 'e o numero de linhas e n o numero de colunas

- Direita: Constante + n x 64 NO EIXO x
- Esquerda: Constante - n x 64 NO EIXO x

- Emcima: Constante + m x 64 NO EIXO y
- Embaixo: Constante - m x 64 NO EIXO y

- Verificar casos de boarda para nao dar overflow na peca dentro do inventario

---

## Continuacao da tool de adicionar itens mas agora para edita-los

Adicionar continuacao da tool de criacao de itens mas agora para poder edita-los
é possivel editar via json mas nao é pratico para game designers, entao uma nova tool
que pudesse pegar a lista de items disponiveis e setar um um campo, e eu poder escolher o item pelo id
e todos os itens aparecessem para edtar e no final um botao de update que alem de atualizar no item manager atualizasse tambem no
json atraves do save manager seria o ideal;

poderia ter usado scritable objects mas acredito que uma tool fica mais intuitivo para um game designer.

---

## Melhorar de forma visual a matriz de mapeamento da peca  

- A matriz de 1 e 0 mapeia os campos da peca que nao sao transparentes
- Seria legal ser possivel adicionar a linha e coluna e elas formassem o formato da matriz visualmente na tool para o game designer,
assim ele so encaixaria cada bloco formando a peca, no exemplo do L ele adicionaria blocos 64x64 da imagem somente onde existe 
blocos na peca em L, seria uma forma mais visual de mapeamento

---

## Salvar os dados de cada slot do inventario ara poder fazer o load deles no inicio do jogo

---

## O ideal seria salvar os dados do player em um servidor e dar load deles a partir disso, com os dados carregados salvar eles no persistent data path que sera o caminho para o computador do usuario

---

## Completar a documentacao, deixa-la mais bonita e salvar em PDF

---


