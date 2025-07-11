import { HistoricoEmprestimo } from "@/types/entities";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";

interface TabelaHistoricoEmprestimosProps {
  historico: HistoricoEmprestimo[];
}

export function TabelaHistoricoEmprestimos({ historico }: TabelaHistoricoEmprestimosProps) {
  return (
    <Table>
      <TableHeader>
        <TableRow>
          <TableHead>Livro</TableHead>
          <TableHead>Data Empréstimo</TableHead>
          <TableHead>Data Devolução</TableHead>
          <TableHead>Dias Atraso</TableHead>
          <TableHead>Multa (R$)</TableHead>
          <TableHead>Observações</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        {historico.map((item) => (
          <TableRow key={item.id_historico}>
            <TableCell>{item.id_livro}</TableCell>
            <TableCell>{item.data_emprestimo}</TableCell>
            <TableCell>{item.data_devolucao}</TableCell>
            <TableCell>{item.dias_atraso}</TableCell>
            <TableCell>{item.multa.toFixed(2)}</TableCell>
            <TableCell>{item.observacoes || "-"}</TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  );
}
